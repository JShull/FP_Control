using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif
namespace FuzzPhyte.Control
{
    /// <summary>
    /// A Class example to utilize the FuzzPhyte Control System
    /// You can also bring in the Samples and just mess with the FP_Controller class
    /// </summary>
    public abstract class FPUnityControl : MonoBehaviour, IFPControl
    {
        [Tooltip("Use old Input System")]
        public bool UseOldInputSystem;
        [Tooltip("This is our main character controller that we will be altering")]
        public CharacterController CharacterControllerComponent;
        [Tooltip("This is our main player transform that we will be moving")]
        public Transform ThePlayer;
        public Transform ThePlayerFeet;
        [Tooltip("Assign the data for the controller via the Scriptable Object")]
        public SO_ControlParameters PlayerControlData;
        
        #region State Control Parameters
        protected bool isRunning;
        protected bool isJumping;
        [SerializeField]
        protected bool isGrounded;
        protected bool hitCeiling;
        protected float curMoveX;
        protected float curMoveZ;
        protected float rotateX;
        protected float rotateY;
        #endregion
        #region Parameters
        protected PlayerController playerController;
        [HideInInspector]
        public Transform Player
        {
            get { return ThePlayer; }
            set { ThePlayer = value; }
        }
        [Tooltip("This is the player main camera")]
        public Camera ThePlayerCamera;
        [HideInInspector]
        public Camera Camera { get { return ThePlayerCamera; } set { ThePlayerCamera = value; } }
        protected float walkingSpeed;
        protected float runningSpeed;
        protected float jumpSpeed;
        protected float jumpHeight;
        protected float gravity;
        protected float radius;
        protected float lookSpeed;
        protected float lookXLimit;
        protected float airJumpScale;
        protected Vector3 moveDirection = Vector3.zero;
        protected float rotationX = 0;
        protected bool canMove = false;
        protected bool canRun = false;
        protected bool canJump = false;
        protected bool inverseLook = true;
        protected bool disableMouseLocking = false;
        protected bool moveWhileFalling;
        protected bool useDataStartPosition;
        #endregion
        #region Interface Requirements
        public float WalkingSpeed { get { return walkingSpeed; } }

        public float RunningSpeed { get { return runningSpeed; } }

        public float JumpSpeed { get { return jumpSpeed; } }

        public float JumpHeight { get { return jumpHeight; } }

        public float GravityScale { get { return gravity; } }

        public float Radius { get { return radius; } set { radius = value; } }

        public float LookSpeed { get { return lookSpeed; } }

        public float LookXLimit { get { return lookXLimit; } }

        public float InAirMotionScale { get { return airJumpScale; } }

        public float3 MoveDirection { get { return moveDirection; } }

        [HideInInspector]
        public float3 PlayerPosition
        {
            get { return ThePlayer.position; }
            set { ThePlayer.position = new Vector3(value.x, value.y, value.z); }
        }
        public float RotationX { get { return rotationX; } set { rotationX = value; } }
        public bool CanMove { get { return canMove; } set { canMove = value; } }
        public bool CanRun { get { return canRun; } set { canRun = value; } }
        public bool CanJump { get { return canJump; } set { canJump = value; } }
        public bool InverseLook { get { return inverseLook; } set { inverseLook = value; } }
        public bool DisableMouseLocking { get { return disableMouseLocking; } set { disableMouseLocking = value; } }

        public bool MoveWhileFalling { get { return moveWhileFalling; } }

        public bool MoveToDataStartPosition { get { return useDataStartPosition; } }



        [HideInInspector]
        public CharacterController CController
        {
            get { return CharacterControllerComponent; }
            set { CharacterControllerComponent = value; }
        }
       
        public virtual void SetupControlData(SO_ControlParameters data)
        {
            //pass the data from the SO_ControlParameters to my local variables
            walkingSpeed = data.WalkingSpeed;
            runningSpeed = data.RunningSpeed;
            jumpSpeed = data.JumpSpeed;
            gravity = data.GravityScale;
            lookSpeed = data.LookSpeed;
            lookXLimit = data.LookXLimit;
            canMove = data.CanMove;
            canRun = data.CanRun;
            canJump = data.CanJump;
            inverseLook = data.InverseLook;
            disableMouseLocking = data.DisableMouseLocking;
            jumpHeight = data.JumpHeight;
            radius = data.CapsuleRadius;
            CharacterControllerComponent.height = data.CapsuleHeight;
            CharacterControllerComponent.center = data.CharacterCenter;
            CharacterControllerComponent.slopeLimit = data.SlopeLimit;
            CharacterControllerComponent.skinWidth = data.SkinWidth;
            CharacterControllerComponent.stepOffset = data.StepOffset;
            moveWhileFalling = data.MoveWhileFalling;
            useDataStartPosition = data.MoveToDataStartPosition;
            airJumpScale = data.InAirJumpScale;
            CharacterControllerComponent.radius = radius;
            if (useDataStartPosition)
            {
                PlayerPosition = data.PlayerPosition;
            }
            else
            {
                PlayerPosition = this.transform.position;
            }
            rotationX = 0;
        }

        #endregion
        #region Unity Methods
        public virtual void Awake()
        {
            SetupControlData(PlayerControlData);
            playerController = new PlayerController(this);
            if (ThePlayerFeet != null)
            {
                ThePlayerFeet.localPosition = new Vector3(0, -CharacterControllerComponent.height / 2, 0);
                
            }
        }
        public virtual void Update()
        {
            isGrounded = CheckGroundSphere();
            hitCeiling = CheckHeadCollisionRaycast();

            //get input from the player
            if (UseOldInputSystem)
            {
                OldInputMethods();
            }
            ApplyDataToController();
        }
        public virtual void LateUpdate()
        {
           
        }
        #endregion
        #region Input Methods
        public virtual void OldInputMethods()
        {
            isRunning = Input.GetKey(KeyCode.LeftShift);
            isJumping = Input.GetKeyDown(KeyCode.Space);
            curMoveX = Input.GetAxis("Vertical");
            curMoveZ = Input.GetAxis("Horizontal");
            rotateX = Input.GetAxis("Mouse X");
            rotateY = Input.GetAxis("Mouse Y");
        }

#if ENABLE_INPUT_SYSTEM

        /// <summary>
        /// Event Driven input for Running
        /// </summary>
        /// <param name="runningValue"></param>
        public void OnRunning(InputValue runningValue)
        {
            if (runningValue.Get<float>() > 0)
            {
                isRunning = true;
            }
            else
            {
                isRunning = false;
            }

            //isRunning = movementValue.Get<bool>();
        }
        public void ContextRunning(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                isRunning = true;
            }
            else
            {
                isRunning = false;
            }
        }
        /// <summary>
        /// We are screening the jump on the PlayerController so we can just contiously get this input
        /// </summary>
        /// <param name="jumpingValue"></param>
        public void OnJumping(InputValue jumpingValue)
        {
            if (jumpingValue.Get<float>() > 0)
            {
                isJumping = true;
            }
            else
            {
                isJumping = false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void ContextJumping(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                isJumping = true;
            }
            else
            {
                isJumping = false;
            }
        }
        /// <summary>
        /// Event Driven input for moving
        /// forward, backward, left, right
        /// </summary>
        /// <param name="movementValue"></param>
        public void OnMoving(InputValue movementValue)
        {
            Vector2 movementVector = movementValue.Get<Vector2>();
            curMoveX = movementVector.y;
            curMoveZ = movementVector.x;
        }
        /// <summary>
        /// if we want to use context
        /// </summary>
        /// <param name="context"></param>
        public void ContextMoving(InputAction.CallbackContext context)
        {
            Vector2 movementVector = context.ReadValue<Vector2>();
            curMoveX = movementVector.y;
            curMoveZ = movementVector.x;
        }
        /// <summary>
        /// Event Driven input for looking with mouse/touch screen
        /// </summary>
        /// <param name="lookValue"></param>
        public void OnLooking(InputValue lookValue)
        {
            Vector2 lookV = lookValue.Get<Vector2>();
            rotateX = lookV.x;
            rotateY = lookV.y;
        }
        /// <summary>
        /// If we want to use Context
        /// </summary>
        /// <param name="context"></param>
        public void ContextLooking(InputAction.CallbackContext context)
        {
            Vector2 lookV = context.ReadValue<Vector2>();
            rotateX = lookV.x;
            rotateY = lookV.y;
        }
#endif
#endregion
        #region Control Data Methods
        protected virtual bool CheckGroundSphere()
        {
            
            return Physics.CheckSphere(
                ThePlayerFeet.transform.position+new Vector3(0,radius- (CharacterControllerComponent.skinWidth*1.1f), 0), 
                radius, 
                PlayerControlData.GroundLayerMask);
        }
        
        protected virtual bool CheckHeadCollisionRaycast()
        {
            float length = (CharacterControllerComponent.height / 2f) + (CharacterControllerComponent.skinWidth * 1.1f);
            return Physics.Raycast(CharacterControllerComponent.bounds.center, Vector3.up, length, PlayerControlData.GroundLayerMask);

        }
        protected virtual void ApplyDataToController()
        {
            moveDirection = playerController.Move(curMoveX, curMoveZ, isJumping, isGrounded, isRunning, hitCeiling, Time.deltaTime);
            playerController.Rotate(new Vector2(rotateX, rotateY));
        }
        #endregion
        public virtual void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(ThePlayerFeet.transform.position + new Vector3(0, radius - (CharacterControllerComponent.skinWidth * 1.1f)), radius);
            Gizmos.color = Color.black;
            float length = (CharacterControllerComponent.height / 2f) + (CharacterControllerComponent.skinWidth * 1.1f);
            Gizmos.DrawLine(CharacterControllerComponent.bounds.center, (CharacterControllerComponent.bounds.center + (Vector3.up * length)));

        }
    }
}
