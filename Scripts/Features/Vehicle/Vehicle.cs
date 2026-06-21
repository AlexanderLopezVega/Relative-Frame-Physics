using System;
using System.Linq;
using UnityEngine;

namespace Project
{
    [RequireComponent(typeof(Rigidbody))]
    public class Vehicle : MonoBehaviour
    {
        //  Inspector
        [Header("Options")]
        [SerializeField] private VehicleController.Options _controllerOptions;

        //  Fields
        private VehicleController _controller;

        //  Methods
        private void Awake()
        {
            _controller = new(_controllerOptions, GetComponent<Rigidbody>(), GetComponentInChildren<BoxCollider>());
        }
        private void FixedUpdate()
        {
            _controller.OnFixedUpdate(Time.fixedDeltaTime);
        }

        private Bounds? GenerateCompoundBounds(Collider[] colliders)
        {
            if (colliders.Length < 1)
                return new();

            Bounds firstBounds = colliders[0].bounds;
            Bounds compoundBounds = new(firstBounds.center, firstBounds.size);

            for (int i = 1; i < colliders.Length; i++)
            {
                Bounds colliderBounds = colliders[i].bounds;

                compoundBounds.SetMinMax(Vector3.Min(compoundBounds.min, colliderBounds.min), Vector3.Max(compoundBounds.max, colliderBounds.max));
            }

            return compoundBounds;
        }
    }
}
