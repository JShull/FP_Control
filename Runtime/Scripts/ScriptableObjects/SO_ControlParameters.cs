using System;
using Unity.Mathematics;
using UnityEngine;

namespace FuzzPhyte.Control
{
    [Serializable]
    [CreateAssetMenu(fileName = "ControlParameters",menuName = "ScriptableObjects/FuzzPhyte/Control/ControlParameters",order =0)]
    public class SO_ControlParameters : ScriptableObject
    {
        [Header("Movement")]
        public float WalkingSpeed = 2f;
        public float RunningSpeed = 6f;
        public float JumpSpeed = 8f;
        public float JumpHeight = 1f;
        public float GravityScale = 1f;
        [Range(0.01f,1f)]
        public float InAirJumpScale = 0.1f;
        [Header("Location")]
        public float3 PlayerPosition;
        [Header("Look")]
        public float LookSpeed = 2f;
        public float LookXLimit = 45f;
        [Header("Player Capsule Size")]
        public float CapsuleRadius=0.5f;
        public float CapsuleHeight=1f;
        public Vector3 CharacterCenter = new Vector3(0, 0.5f, 0);
        public float SlopeLimit = 45;
        public float SkinWidth = 0.01f;
        public float StepOffset = 0.1f;
        [Header("Misc")]
        public bool CanMove = true;
        public bool CanJump = false;
        public bool CanRun = false;
        public bool InverseLook = false;
        public bool DisableMouseLocking = false;
        public bool MoveWhileFalling = false;
        [Header("LayerMasking Needs")]
        public LayerMask GroundLayerMask;
        //public QueryTriggerInteraction GroundLayerMask;
    }

}
