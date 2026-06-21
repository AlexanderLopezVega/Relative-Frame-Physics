using UnityEngine;

namespace Project
{
    public class AirState : CharacterControllerState
    {
        //  Constructors
        public AirState(CharacterController context) : base(context) { }

        //  Methods
        public override void OnBeforeUpdate()
        {
            if (_context.GroundingStatus.Current.FoundAnyGround)
                _context.State = _context.Factory.CreateGroundState();
        }
        public override void UpdateLinearVelocity(ref Vector3 velocity, float fixedDeltaTime)
        {
            DecomposeVelocity(velocity, out Vector3 planarVelocity, out Vector3 verticalVelocity);

            Vector3 targetPlanarVelocity = planarVelocity + _context.Options_.Acceleration * fixedDeltaTime * GetTargetDirection();
            
            planarVelocity = Vector3.ClampMagnitude(
                vector: targetPlanarVelocity,
                maxLength: Mathf.Max(
                    planarVelocity.magnitude,
                    _context.Options_.Speed
                )
            );

            velocity = planarVelocity + verticalVelocity;
        }
    }
}
