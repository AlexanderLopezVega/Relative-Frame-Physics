using UnityEngine;

namespace Project
{
    public class GroundState : CharacterControllerState
    {
        //  Fields
        private bool _jumpFlag;

        //  Constructors
        public GroundState(CharacterController context) : base(context) { }

        //  Methods
        public override void OnJump()
        {
            _jumpFlag = true;
        }
        public override void OnBeforeUpdate()
        {
            if (!_context.GroundingStatus.Current.FoundAnyGround)
                _context.State = _context.Factory.CreateAirState();
        }
        public override void UpdateLinearVelocity(ref Vector3 velocity, float fixedDeltaTime)
        {
            DecomposeVelocity(velocity, out Vector3 planarVelocity, out Vector3 verticalVelocity);

            planarVelocity = Vector3.MoveTowards(
                current: planarVelocity,
                target: _context.Options_.Speed * GetTargetDirection(),
                maxDistanceDelta: _context.Options_.Acceleration * fixedDeltaTime
            );

            if (_jumpFlag)
            {
                _jumpFlag = false;
                verticalVelocity = Mathf.Sqrt(Physics.gravity.magnitude * 2f * _context.Options_.JumpHeight) * Vector3.up;
            }

            velocity = planarVelocity + verticalVelocity;
        }
    }
}
