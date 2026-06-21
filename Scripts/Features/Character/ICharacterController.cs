using UnityEngine;

namespace Project
{
    public interface ICharacterController
    {
        //  Methods
        void OnBeforeUpdate();
        void UpdateLinearVelocity(ref Vector3 velocity, float fixedDeltaTime);
        void UpdateRotation(ref Quaternion rotation, float fixedDeltaTime);
        void UpdateCameraRotation(ref Quaternion localRotation, float deltaTime);
    }
}
