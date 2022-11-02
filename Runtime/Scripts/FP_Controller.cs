using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
//If you're using the new input system
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
#endif
namespace FuzzPhyte.Control
{
    public class FP_Controller : MonoBehaviour, IFPControl
    {
        [Tooltip("Assign the data for the controller via the Scriptable Object")]
        public SO_ControlParameters PlayerControlData;
        private PlayerController _playerController;
        [Tooltip("This is our main player transform that we will be moving")]
        public Transform ThePlayer;
        [Tooltip("This is the player main camera")]
        public Camera ThePlayerCamera;
        [HideInInspector]
        public Camera Camera
        {
            get { return ThePlayerCamera; }
            set { ThePlayerCamera = value; }
        }
        [HideInInspector]
        public Transform Player
        {
            get { return ThePlayer; }
            set { ThePlayer = value; }
        }
        [Tooltip("This is our main character controller that we will be altering")]
        public CharacterController CharacterControllerComponent;
        [HideInInspector]
        public CharacterController CController
        {
            get { return CharacterControllerComponent; }
            set { CharacterControllerComponent = value; }
        }
        [HideInInspector]
        public float3 PlayerPosition
        {
            get { return ThePlayer.position; }
            set { ThePlayer.position = new Vector3(value.x,value.y,value.z); }
        }
        
#region Interface Parameter Setup
        float walkingSpeed;
        public float WalkingSpeed { get { return walkingSpeed; } }
        float runningSpeed;
        public float RunningSpeed { get { return runningSpeed; } }
        float jumpSpeed;
        public float JumpSpeed { get { return jumpSpeed; } }
        float airJumpScale;
        public float InAirMotionScale { get { return airJumpScale; } }
        float gravity;
        public float GravityScale { get { return gravity; } }
        float lookSpeed;
        public float LookSpeed { get { return lookSpeed; } }
        float lookXLimit;
        public float LookXLimit { get { return lookXLimit; } }
        public float JumpHeight { get { return jumpHeight; } }
        float jumpHeight;
        Vector3 moveDirection = new Vector3();
        public float3 MoveDirection { get { return moveDirection; } }
        float rotationX;
        public float RotationX { get { return rotationX; } set{ rotationX = value;  } }  
        float radius;
        public float Radius { get { return radius; } set { radius = value; } }
        bool canMove;
        public bool CanMove { get { return canMove; } set { canMove = value; } }
        bool canJump;
        public bool CanJump { get { return canJump; } set { canJump = value; } }
        bool canRun;
        public bool CanRun { get { return canRun; } set { canRun = value; } }
        bool moveWhileFalling;
        public bool MoveWhileFalling { get { return moveWhileFalling; } }
        bool inverseLook;
        public bool InverseLook { get { return inverseLook; } set { inverseLook = value; } } 
        bool disableMouseLocking;
        public bool DisableMouseLocking { get { return disableMouseLocking; }set { disableMouseLocking = value; } }
        #endregion

        #region Input and Motion Specific Local Variables
#if ENABLE_INPUT_SYSTEM
        [Tooltip("Just a reference to the current player input actions")]
        public PlayerInput PlayerInputSystem;
#endif
        private bool _runPressed;
        private bool _runReleased;
        //public  Controls;
        private bool isRunning;
        private bool isJumping;
        private bool isGrounded;
        private float curMoveX;
        private float curMoveZ;
        private float rotateX;
        private float rotateY;
#endregion
        private void Awake()
        {
            SetupControlData(PlayerControlData);
            _playerController = new PlayerController(this);
        }
        /// <summary>
        /// This interface method can be thought of as a constructor, should be called via OnEnable/Awake/Start
        /// </summary>
        /// <param name="data"></param>
        public void SetupControlData(SO_ControlParameters data)
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
            airJumpScale = data.InAirJumpScale;
            CharacterControllerComponent.radius = radius;
            PlayerPosition = data.PlayerPosition;
            rotationX = 0;
        }
        
        /// <summary>
        /// Used to get all inputs via new/old input manager system
        /// </summary>
        public void Update()
        {
            //we assume new input system, if not then disable both and only enable the old one
#if ENABLE_INPUT_SYSTEM

#else
            isRunning = Input.GetKey(KeyCode.LeftShift);
            isJumping = Input.GetKeyDown(KeyCode.Space);
            curMoveX = Input.GetAxis("Vertical");
            curMoveZ = Input.GetAxis("Horizontal");
            rotateX = Input.GetAxis("Mouse X");
            rotateY = Input.GetAxis("Mouse Y");
#endif
            _playerController.Move(curMoveX, curMoveZ, isJumping, isGrounded, isRunning, Time.deltaTime);
            _playerController.Rotate(new Vector2(rotateX, rotateY));
            
#if ENABLE_INPUT_SYSTEM
#endif
        }
        public void FixedUpdate()
        {
            //This only works if we are not colliding with ourselves - this is a super simple way to ground check but make sure
            //your player and your ground are on different layers!!!
            isGrounded = Physics.CheckSphere(CharacterControllerComponent.bounds.center,
                (CharacterControllerComponent.height/2)+ CharacterControllerComponent.skinWidth*1.1f,
                PlayerControlData.GroundLayerMask);
            
        }
        #region New Unity Input Logic
#if ENABLE_INPUT_SYSTEM
        /// <summary>
        /// Event Driven input for Running
        /// </summary>
        /// <param name="runningValue"></param>
        public void OnRunning(InputValue runningValue)
        {
            if (runningValue.Get<float>() > 0) {
                isRunning = true;
            }
            else
            {
                isRunning = false;
            }
           
            //isRunning = movementValue.Get<bool>();
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
        /// Event Driven input for looking with mouse/touch screen
        /// </summary>
        /// <param name="lookValue"></param>
        public void OnLooking(InputValue lookValue)
        {
            Vector2 lookV = lookValue.Get<Vector2>();
            rotateX = lookV.x;
            rotateY = lookV.y;
        }
#endif
#endregion
    }
}

