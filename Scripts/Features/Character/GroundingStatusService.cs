using System;
using UnityEngine;

namespace Project
{
    public class GroundingStatusService
    {
        //  Constants
        private const uint ResultsBudget = 32;

        //  Fields
        private readonly Options _options;
        private readonly CapsuleCollider _collider;
        private readonly RaycastHit[] _results;

        //  Constructors
        public GroundingStatusService(Options options, CapsuleCollider collider, uint resultsBudget = ResultsBudget)
        {
            _options = options;
            _collider = collider;
            _results = new RaycastHit[resultsBudget];
        }

        //  Constructors

        //  Properties
        public GroundingStatus Previous { get; private set; }
        public GroundingStatus Current { get; private set; }

        //  Methods
        public void OnFixedUpdate()
        {
            (Previous, Current) = (Current, CalculateGroundingStatus());
        }

        private GroundingStatus CalculateGroundingStatus()
        {
            GroundingStatus status = new();
            Vector3 start = _collider.transform.TransformPoint(_collider.center - _collider.height * 0.5f * _collider.transform.up);
            Vector3 end = start - _options.MaxDistance * _collider.transform.up;
            int numHits = Physics.CapsuleCastNonAlloc(
                point1: start,
                point2: end,
                radius: _collider.radius,
                direction: -_collider.transform.up,
                results: _results,
                maxDistance: _options.MaxDistance,
                layerMask: _options.LayerMask
            );

            if (numHits == 0)
                return status;
                
            uint validColliders = 0;

            for (int i = 0; i < numHits; ++i)
            {
                if (_results[i].collider == _collider)
                    continue;

                status.GroundNormal += _results[i].normal;
                ++validColliders;
            }
            
            status.GroundNormal /= validColliders;
            status.FoundAnyGround = validColliders > 0;

            return status;
        }

        //  Classes
        [Serializable]
        public class Options
        {
            //  Inspector
            [field: SerializeField, Min(0f)] public float MaxDistance { get; private set; } = 0.01f;
            [field: SerializeField] public LayerMask LayerMask { get; private set; }
        }
    }
}
