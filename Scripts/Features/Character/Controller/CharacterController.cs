using System;
using UnityEngine;

namespace Project
{
    public class CharacterController
    {
        //  Fields
        private readonly CharacterMotor _motor;
        private CharacterControllerState _state;

        //  Constructors
        public CharacterController(Options options, Transform root, CharacterMotor motor)
        {
            _motor = motor;
            Options_ = options;
            Root = root;
            Factory = new(this);
        }

        //  Properties
        public Options Options_ { get; }
        public Transform Root { get; }
        public CharacterControllerStateFactory Factory { get; }
        public GroundingStatusService GroundingStatus => _motor.GroundingStatus;
        public CharacterControllerState State { set => _motor.Controller = _state = value; }
        public Vector2 MoveInput { get; set; }
        public Vector2 LookInput { get; set; }
        public Vector2 LookAngles { get => _motor.LookAngles; set => _motor.LookAngles = value; }

        //  Methods
        public void OnUpdate(float deltaTime) => _state.OnUpdate(deltaTime);

        public void OnJump() => _state.OnJump();

        //  Classes
        [Serializable]
        public class Options
        {
            //  Inspector
            [field: SerializeField, Min(0f)] public float Speed { get; set; } = 3f;
            [field: SerializeField, Min(0f)] public float Acceleration { get; set; } = 3f;
            [field: Space]
            [field: SerializeField, Min(0f)] public float Sensitivity { get; set; } = 1f;
            [field: SerializeField] public bool InvertPitch { get; set; } = true;
            [field: SerializeField] public bool InvertYaw { get; set; } = false;
            [field: Space]
            [field: SerializeField, Min(0.1f)] public float JumpHeight { get; private set; } = 1f;
        }
    }
}
