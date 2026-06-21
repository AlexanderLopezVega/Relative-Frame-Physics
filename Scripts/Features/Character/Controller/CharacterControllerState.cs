using System;
using UnityEngine;

namespace Project
{
    public abstract class CharacterControllerState : ICharacterController
    {
        //  Constants
        private const float BaseSensitivity = 3f;

        //  Fields
        protected readonly CharacterController _context;

        //  Constructors
        protected CharacterControllerState(CharacterController context)
        {
            _context = context;
        }

        //  Interface implementations
        public virtual void OnBeforeUpdate() { }
        public virtual void UpdateLinearVelocity(ref Vector3 velocity, float fixedDeltaTime) { }
        public void UpdateRotation(ref Quaternion rotation, float fixedDeltaTime)
        {
            rotation = Quaternion.Euler(0f, _context.LookAngles.x, 0f);
        }
        public void UpdateCameraRotation(ref Quaternion localRotation, float deltaTime)
        {
            localRotation = Quaternion.Euler(_context.LookAngles.y, 0f, 0f);
        }

        //  Methods
        public void OnUpdate(float deltaTime)
        {
            float yaw = _context.LookAngles.x;
            float pitch = _context.LookAngles.y;

            yaw += _context.LookInput.x * BaseSensitivity * _context.Options_.Sensitivity * (_context.Options_.InvertYaw ? -1f : 1f) * deltaTime;
            pitch += _context.LookInput.y * BaseSensitivity * _context.Options_.Sensitivity * (_context.Options_.InvertPitch ? -1f : 1f) * deltaTime;

            if (yaw > 180f)         yaw -= 360f;
            else if (yaw < -180f)   yaw += 360f;
            
            pitch = Mathf.Clamp(pitch, -90f, 90f);

            _context.LookAngles = new(yaw, pitch);
        }
        public virtual void OnJump() { }

        protected Vector3 GetTargetDirection() => _context.Root.TransformDirection(new(_context.MoveInput.x, 0f, _context.MoveInput.y));
        protected void DecomposeVelocity(Vector3 velocity, out Vector3 planarVelocity, out Vector3 verticalVelocity)
        {
            planarVelocity = Vector3.ProjectOnPlane(velocity, Vector3.up);
            verticalVelocity = Vector3.Project(velocity, Vector3.up);
        }
    }
}
