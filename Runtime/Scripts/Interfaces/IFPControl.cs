using FuzzPhyte.Control;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public interface IFPControl
{
    float WalkingSpeed { get; }
    float RunningSpeed { get; }
    float JumpSpeed { get; }
    float JumpHeight { get; }
    float GravityScale { get; }
    float Radius { get; set; }
    float LookSpeed { get; }
    float LookXLimit { get; }
    float InAirMotionScale { get;}
    float3 MoveDirection { get;}
    float3 PlayerPosition { get; set; }
    float RotationX { get; set; }
    bool CanMove { get; set; }
    bool CanRun { get; set; }
    bool CanJump { get; set; }
    bool InverseLook { get; set; }
    bool DisableMouseLocking { get; set; }
    bool MoveWhileFalling { get; }
    bool MoveToDataStartPosition { get; }
    Camera Camera { get; set; }
    Transform Player { get; set; }
    CharacterController CController { get; set; }
    /// <summary>
    /// Setup our player with the data from our Scriptable Object
    /// </summary>
    /// <param name="data"></param>
    public void SetupControlData(SO_ControlParameters data);
}
