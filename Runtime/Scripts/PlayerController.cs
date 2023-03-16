using UnityEngine;
namespace FuzzPhyte.Control
{
    /// <summary>
    /// This class is following a Humble Object pattern. 
    /// It is a simple class that is used to store data and actions by data
    /// </summary>
    public class PlayerController
    {
        protected IFPControl _control;
        private const float defaultTime = 0.2f;
        protected Vector3 moveDirection = Vector3.zero;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="control"></param>
        public PlayerController(IFPControl control)
        {
            _control = control;
        }
        protected float jumpMoveX=0;
        protected float jumpMoveZ=0;
        
        protected Vector3 forward;
        protected Vector3 right;
        protected Vector3 forwardJump;
        protected Vector3 rightJump;

        #region Actions/Systems
        /// <summary>
        /// When we need to move our player controller
        /// We need the input information as a Vector3
        /// </summary>
        /// <param name="moveDirection">X is right, Y is up, & Z is forward, </param>
        /// <param name="jumping">Is the Jump Input being activated</param>
        /// <param name="running">Is the Run Input being activated</param>
        /// <param name="updateTick">Update rate for this action, Time.deltaTime or Time.fixedDeltaTime or custom</param>
        public virtual void Move(float moveX, float moveZ, bool jumping, bool grounded, bool running, float updateTick = defaultTime)
        {
            if (_control.CanMove)
            {
                forward = _control.Player.TransformDirection(Vector3.forward);
                right = _control.Player.TransformDirection(Vector3.right);
                //jump first
                if (_control.CanJump && grounded && jumping)
                {
                    //force model for jumping by height with a scale adjustment for speed
                    moveDirection.y = Mathf.Sqrt(-2f * _control.JumpSpeed * (_control.JumpHeight - (3f * _control.CController.skinWidth)) * _control.GravityScale * Physics.gravity.y);
                    jumpMoveX = moveX;
                    jumpMoveZ = moveZ;
                    forwardJump = forward;
                    rightJump = right;
                   
                }
                if (grounded)
                {
                    UpdateMove(running, moveX, moveZ);
                    moveDirection = (forward * moveDirection.x) + new Vector3(0, moveDirection.y, 0) + (right * moveDirection.z);
                }
                else
                {
                    //apply gravity on fall if we aren't grounded
                    moveDirection.y += _control.GravityScale * Physics.gravity.y*updateTick;
                    if (!grounded && !_control.MoveWhileFalling)
                    {
                        UpdateMove(running, jumpMoveX, jumpMoveZ);
                        moveDirection = (forwardJump * moveDirection.x) + new Vector3(0, moveDirection.y, 0) + (rightJump * moveDirection.z);
                    }
                    if (!grounded && _control.MoveWhileFalling)
                    {
                        UpdateMove(running, jumpMoveX+(moveX * _control.InAirMotionScale), jumpMoveZ+(moveZ* _control.InAirMotionScale));
                        moveDirection = (forward * moveDirection.x) + new Vector3(0, moveDirection.y, 0) + (right * moveDirection.z);
                    }
                }
                //apply to character controller 
                _control.CController.Move(moveDirection * updateTick);
            }
        }
        /// <summary>
        /// Generic X/Z Motion and movement
        /// </summary>
        /// <param name="run"></param>
        /// <param name="mX"></param>
        /// <param name="mZ"></param>
        public virtual void UpdateMove(bool run, float mX,float mZ)
        {
            if (run && _control.CanRun)
            {
                //running motion
                moveDirection.x = _control.RunningSpeed * mX;
                moveDirection.z = _control.RunningSpeed * mZ;
            }
            else
            {
                //normal motion
                moveDirection.x = _control.WalkingSpeed * mX;
                moveDirection.z = _control.WalkingSpeed * mZ;
            }
        }
        /// <summary>
        /// When we need to rotate our player controller
        /// We need the input information as a Vector2
        /// We lock the rotation of the character controller to only rotate on the Y axis
        /// We then rotate the camera within it's limits across the X axis
        /// </summary>
        /// <param name="rotation">X should be Mouse Y, Y should be Mouse X</param>
        public virtual void Rotate(Vector2 rotation)
        {
            if (_control.CanMove)
            {
                if (_control.InverseLook)
                {
                    _control.RotationX += -rotation.y * _control.LookSpeed;
                }
                else
                {
                    _control.RotationX += rotation.y * _control.LookSpeed;
                }
                _control.RotationX = Mathf.Clamp(_control.RotationX, -_control.LookXLimit, _control.LookXLimit);
                _control.Camera.transform.localRotation = Quaternion.Euler(_control.RotationX, 0, 0);
                _control.Player.rotation *= Quaternion.Euler(0, rotation.x * _control.LookSpeed, 0);
            }
        }
        /// <summary>
        /// Quickly jump my player to an X coordinate
        /// </summary>
        /// <param name="xValue"></param>
        public virtual void MovePlayerX(float xValue)
        {
            var curPos = _control.PlayerPosition;
            _control.PlayerPosition = new Vector3(xValue, curPos.y, curPos.z);
        }
        /// <summary>
        /// Quickly jump my player to a Z coordinate
        /// </summary>
        /// <param name="zValue"></param>
        public virtual void MovePlayerZ(float zValue)
        {
            var curPos = _control.PlayerPosition;
            _control.PlayerPosition = new Vector3(curPos.x, curPos.y, zValue);
        }
        /// <summary>
        /// Quickly jump my player to a Y coordinate
        /// </summary>
        /// <param name="yValue"></param>
        public virtual void MovePlayerY(float yValue)
        {
            var curPos = _control.PlayerPosition;
            _control.PlayerPosition = new Vector3(curPos.x, yValue, curPos.z);
        }
        /// <summary>
        /// Generally called when we need to disable the controller
        /// </summary>
        public virtual void LockPlayerMotion()
        {
            _control.CanMove = false;
            _control.DisableMouseLocking = false;
        }
        /// <summary>
        /// Generally called when we need to enable the controller
        /// </summary>
        public virtual void UnlockPlayerMotion()
        {
            _control.CanMove = true;
            _control.DisableMouseLocking = true;
        }
        /// <summary>
        /// Do we need to show the cursor?
        /// </summary>
        public virtual void ShowCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        /// <summary>
        /// Do we need to hide the cursor?
        /// </summary>
        public virtual void HideCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
        #endregion
    }


}
