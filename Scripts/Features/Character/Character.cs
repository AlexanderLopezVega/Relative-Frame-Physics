using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project
{
    [RequireComponent(typeof(Rigidbody))]
    public class Character : MonoBehaviour
    {
        //  Inspector
        [Header("Dependencies")]
        [SerializeField] private Transform _cameraRoot;

        [Header("Options")]
        [SerializeField] private CharacterController.Options _controllerOptions;
        [SerializeField] private GroundingStatusService.Options _groundingStatusOptions;

        //  Fields
        private CharacterMotor _motor;
        private CharacterController _controller;
        private GroundingStatusService _groundingStatus;

        //  Methods
        private void Awake()
        {
            _groundingStatus = new(
                options: _groundingStatusOptions,
                collider: GetComponentInChildren<CapsuleCollider>()
            );
            _motor = new(
                    rigidbody: GetComponent<Rigidbody>(),
                    cameraRoot: _cameraRoot,
                    groundingStatus: _groundingStatus
                );
            _controller = new(
                options: _controllerOptions,
                root: transform,
                motor: _motor
            );

            _controller.State = _controller.Factory.CreateAirState();
        }
        private void Update()
        {
            float deltaTime = Time.deltaTime;

            _motor.OnUpdate(deltaTime);
            _controller.OnUpdate(deltaTime);
        }
        private void FixedUpdate()
        {
            _groundingStatus.OnFixedUpdate();
            _motor.OnFixedUpdate(Time.fixedDeltaTime);
        }

        public void OnMove(InputValue inputValue)
        {
            _controller.MoveInput = inputValue.Get<Vector2>();
        }
        public void OnLook(InputValue inputValue)
        {
            _controller.LookInput = inputValue.Get<Vector2>();
        }
        public void OnJump(InputValue inputValue)
        {
            _controller.OnJump();
        }
    }
}
