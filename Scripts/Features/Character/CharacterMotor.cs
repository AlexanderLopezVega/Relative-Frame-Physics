using UnityEngine;

namespace Project
{
    public class CharacterMotor
    {
        //  Fields
        private readonly Rigidbody _rigidbody;
        private readonly Transform _cameraRoot;

        //  Constructors
        public CharacterMotor(Rigidbody rigidbody, Transform cameraRoot, GroundingStatusService groundingStatus)
        {
            _rigidbody = rigidbody;
            _cameraRoot = cameraRoot;
            GroundingStatus = groundingStatus;
        }

        //  Properties
        public GroundingStatusService GroundingStatus { get; }
        public ICharacterController Controller { get; set; }
        public Vector2 LookAngles { get; set; }

        //  Methods
        public void OnUpdate(float deltaTime)
        {
            UpdateCameraRotation(deltaTime);
        }
        public void OnFixedUpdate(float fixedDeltaTime)
        {
            Controller.OnBeforeUpdate();
            UpdateLinearVelocity(fixedDeltaTime);
            UpdateRotation(fixedDeltaTime);
        }

        private void UpdateCameraRotation(float deltaTime)
        {
            Quaternion localRotation = _cameraRoot.localRotation;
            Controller.UpdateCameraRotation(ref localRotation, deltaTime);
            _cameraRoot.localRotation = localRotation;
        }
        private void UpdateLinearVelocity(float fixedDeltaTime)
        {
            Vector3 linearVelocity = _rigidbody.linearVelocity;
            Controller.UpdateLinearVelocity(ref linearVelocity, fixedDeltaTime);
            _rigidbody.linearVelocity = linearVelocity;
        }
        private void UpdateRotation(float fixedDeltaTime)
        {
            Quaternion rotation = _rigidbody.rotation;
            Controller.UpdateRotation(ref rotation, fixedDeltaTime);
            _rigidbody.MoveRotation(rotation);
        }
    }
}
