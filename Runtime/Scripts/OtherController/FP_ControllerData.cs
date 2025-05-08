namespace FuzzPhyte.Control
{
    using UnityEngine;
    using FuzzPhyte.Utility;

    [CreateAssetMenu(fileName = "FP_ControlData", menuName = "FuzzPhyte/Control/FP_ControlData")]
    public class FP_ControllerData : FP_Data
    {
        [Header("Movement Settings")]
        public float MovementSpeed = 5f;
        public float JumpHeight = 2f;
        public float CharacterHeight = 2f;
        public float CharacterMass=2f;
        public float CrouchHeight = 1f;
        public float CharacterRadius = 0.5f;
        public float SlideSpeedMultiplier = 1.5f;
        [Space]
        [Header("Surface Settings")]
        public bool AllowSurfaceLock = true;
        public float SurfaceLockRayDistance = 2f;
        public float SurfaceLockLerpTime=5f;
        public LayerMask SurfaceLayerMask;
        [Space]
        [Header("Physics Settings")]
        public float LinearDamping = 0.1f;
        public float AngularDamping = 0.1f;
      
       

        public bool UseNavMeshByDefault = false;
       
    
    }
}
