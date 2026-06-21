using System;
using UnityEngine;

namespace Project
{
    public class VehicleController
    {
        //  Fields
        private readonly Options _options;
        private readonly Rigidbody _rigidbody;
        private readonly BoxCollider _boxCollider;

        //  Constructors
        public VehicleController(Options options, Rigidbody rigidbody, BoxCollider boxCollider)
        {
            _options = options;
            _rigidbody = rigidbody;
            _boxCollider = boxCollider;
        }

        //  Methods
        public void OnFixedUpdate(float fixedDeltaTime)
        {
            UpdateLinearVelocity(fixedDeltaTime);
        }

        private void UpdateLinearVelocity(float fixedDeltaTime)
        {
            int numRaysX = Mathf.RoundToInt(_boxCollider.size.x / _options.RayDensity) + 1;
            int numRaysZ = Mathf.RoundToInt(_boxCollider.size.z / _options.RayDensity) + 1;

            Vector3 halfSize = _boxCollider.size * 0.5f;

            for (int z = 0; z < numRaysZ; ++z)
            {
                for (int x = 0; x < numRaysX; ++x)
                {
                    Vector3 origin = _rigidbody.transform.TransformPoint(
                        position: new(
                            x: Mathf.Lerp(-halfSize.x, halfSize.x, Mathf.InverseLerp(0, numRaysX - 1, x)),
                            y: 0f,
                            z: Mathf.Lerp(-halfSize.z, halfSize.z, Mathf.InverseLerp(0, numRaysZ - 1, z))
                        )
                    );

                    Debug.DrawRay(origin, Vector3.down, Color.green);

                    bool raycastResult = Physics.Raycast(
                        origin: origin,
                        direction: Vector3.down,
                        hitInfo: out RaycastHit hitInfo,
                        maxDistance: _options.RayMaxDistance,
                        layerMask: _options.RayMask
                    );

                    if (!raycastResult)
                        continue;
                    
                    float height = Vector3.Distance(
                        a: origin,
                        b: hitInfo.point
                    );
                    float displacement = _options.HoverHeight - height;

                    Vector3 springForce = displacement * _options.HoverStrength * Vector3.up;
                    Vector3 dampingForce = _options.HoverDamping * -1f * _rigidbody.GetPointVelocity(origin);

                    _rigidbody.AddForceAtPosition(springForce + dampingForce, origin);
                }
            }
        }

        //  Classes
        [Serializable]
        public class Options
        {
            //  Inspector
            [field: SerializeField, Min(0.1f)] public float RayDensity { get; private set; } = 1;
            [field: SerializeField, Min(0.1f)] public float RayMaxDistance { get; private set; } = 1f;
            [field: SerializeField] public LayerMask RayMask { get; private set; }
            [field: Space]
            [field: SerializeField, Min(0.1f)] public float HoverHeight { get; private set; } = 1f;
            [field: SerializeField, Min(0.1f)] public float HoverStrength { get; private set; } = 1f;
            [field: SerializeField, Min(0.1f)] public float HoverDamping { get; private set; } = 1f;
        }
    }
}
