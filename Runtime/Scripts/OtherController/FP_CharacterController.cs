namespace FuzzPhyte.Control
{
    using UnityEngine;
    using UnityEngine.AI;
    using FuzzPhyte.Utility;

    public class FP_CharacterController : MonoBehaviour,IFPOnStartSetup
    {
        public FP_ControllerData controllerData;
        [SerializeField] protected Rigidbody rb;
        [SerializeField] protected BoxCollider col;
        [SerializeField] protected NavMeshAgent navAgent;
        [SerializeField] protected Transform RaycastOrigin;
        [SerializeField] protected Transform RootController;
        protected bool useGravity = true;
        protected bool isSliding = false;
        protected bool surfaceLockingAllowed = false;
        protected float originalHeight;
        protected float originalRadius;
        protected float crouchHeight;
        protected bool setupStart;
        public bool SetupStart {get=> setupStart; set => setupStart = value;}

        // Cached input values
        public Vector3 MovementInput { get; set; }
        public bool JumpInput { get; set; }
        public bool CrouchInput { get; set; }
        public bool SlideInput { get; set; }
    
        protected IFPControllerState currentState;

        public virtual void Awake()
        {
           //other FP stuff gets setup generally in awake 
        }
        public virtual void Start()
        {
            if(SetupStart)
            {
                Setup();
            }
            currentState = new FP_IdleState(); // Default state
            currentState?.OnEnter(this);
            //currentState = new FP_ControllerState(); // Default state, can be replaced with a specific state
        }

        protected virtual void Setup()
        {
            originalHeight = controllerData.CharacterHeight;
            originalRadius = controllerData.CharacterRadius;
            crouchHeight = controllerData.CrouchHeight;
            
            surfaceLockingAllowed = controllerData.AllowSurfaceLock;
            if(rb==null)
            {
                rb = GetComponent<Rigidbody>();
                if (rb == null)
                {
                    Debug.LogError($"missing Rigidbody on {gameObject.name}, adding one.");
                    rb = gameObject.AddComponent<Rigidbody>();
                }
            }
            if(col==null)
            {
                col = GetComponent<BoxCollider>();
                if (col == null)
                {
                    Debug.LogError($"missing BoxCollider on {gameObject.name}, adding one.");
                    col = gameObject.AddComponent<BoxCollider>();
                }
            }
            col.size = new Vector3(originalRadius*2, originalHeight, originalRadius*2);
            col.center = new Vector3(0, controllerData.CharacterHeight / 2, 0);
            rb.useGravity = useGravity;
            rb.isKinematic = false;
            rb.linearDamping = controllerData.LinearDamping;
            rb.angularDamping = controllerData.AngularDamping;
            rb.mass = controllerData.CharacterMass;
            navAgent = GetComponent<NavMeshAgent>();
            
            if (navAgent != null)
            {
                navAgent.enabled = controllerData.UseNavMeshByDefault;
                navAgent.speed = controllerData.MovementSpeed;
                navAgent.height = controllerData.CharacterHeight;
                navAgent.radius = controllerData.CharacterRadius;
            }
            
        }
        public virtual void SetupWithData(FP_ControllerData data)
        {
            controllerData = data;
            Setup();
        }
        void FixedUpdate()
        {
            currentState?.UpdateState(this);
            if (surfaceLockingAllowed)
                TrySurfaceLock();

            rb.useGravity = useGravity;
        }
        public virtual void ApplyMovement(Vector3 direction)
        {
            Vector3 velocity = direction.normalized * controllerData.MovementSpeed;
            if (isSliding) velocity *= controllerData.SlideSpeedMultiplier;

            if (useGravity)
                rb.AddForce( new Vector3(velocity.x, rb.linearVelocity.y, velocity.z), ForceMode.VelocityChange);
            else
                rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }

        public virtual void Jump()
        {
            if (rb == null) return;

            if (useGravity)
            {
                rb.AddForce(Vector3.up * controllerData.JumpHeight * controllerData.CharacterMass, ForceMode.VelocityChange);
            }
            else
            {
                rb.MovePosition(rb.position + Vector3.up * controllerData.JumpHeight);
            }
        }

        public void ToggleGravity(bool enabled)
        {
            useGravity = enabled;
            rb.useGravity = enabled;
        }

        public void SetCrouch(bool isCrouched)
        {
            col.size = isCrouched ? new Vector3(originalRadius*2,crouchHeight,originalRadius*2f) : new Vector3(originalRadius*2,originalHeight,originalRadius*2f);
            col.center = new Vector3(0, isCrouched ? crouchHeight / 2 : originalHeight / 2, 0);
        }

        public void StartSlide()
        {
            isSliding = true;
            rb.linearDamping = 0f;
        }

        public void StopSlide()
        {
            isSliding = false;
            rb.linearDamping = controllerData.LinearDamping;
        }

        protected void TrySurfaceLock()
        {
            //
            if (Physics.Raycast(RaycastOrigin.position, -RaycastOrigin.up, out RaycastHit hit, controllerData.SurfaceLockRayDistance, controllerData.SurfaceLayerMask))
            {
                Vector3 targetUp = hit.normal;
                Quaternion surfaceRotation = Quaternion.FromToRotation(RootController.up, targetUp) * RootController.rotation;
                RootController.rotation = Quaternion.Slerp(RootController.rotation, surfaceRotation, Time.fixedDeltaTime * 5f);
            }
        }
        public void SetState(IFPControllerState newState)
        {
            currentState?.OnExit(this);
            currentState = newState;
            currentState?.OnEnter(this);
        }
    }
}
