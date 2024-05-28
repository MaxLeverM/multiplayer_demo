using System;
using UnityEngine;

namespace Gameplay
{
    [Serializable]
    public class MovementController
    {
        [Tooltip("Move speed of the character in m/s")]
        [SerializeField] private float MoveSpeed = 100.0f;
        [Tooltip("Sprint speed of the character in m/s")]
        [SerializeField] private float SprintSpeed = 5.335f;
        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        [SerializeField] private float RotationSmoothTime = 0.12f;
        [Tooltip("Acceleration and deceleration")]
        [SerializeField] private float SpeedChangeRate = 10.0f;
        [Space(10)]
        [Tooltip("The height the player can jump")]
        [SerializeField] private float JumpHeight = 1.2f;
        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        [SerializeField] private float Gravity = -15.0f;
        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        [SerializeField] private float JumpTimeout = 0.50f;
        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        [SerializeField] private float FallTimeout = 0.15f;
        [Header("Player Grounded")]
        [SerializeField] private float GroundedOffset = -0.14f;
        [SerializeField] private float GroundedRadius = 0.28f;
        [SerializeField] private LayerMask GroundLayers;

        public event Action<bool> OnGroundedStateChanged;
        public event Action<bool> OnJumpStateChanged;
        public event Action<bool> OnFreeFallStateChanged;
        public event Action<float> OnAnimationBlendChanged;
        public event Action<float> OnInputMagnitudeChanged;
        
        [SerializeField] private Transform _transform;
        [SerializeField] private CharacterController _controller;
        [SerializeField] private StarterAssetsInputs _input;
        private Camera _mainCamera;

        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;
        private bool _isGrounded = true;
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;
        
        public float GetGroundedOffset => GroundedOffset;

        public float GetGroundedRadius => GroundedRadius;

        public bool IsGrounded => _isGrounded;

        public MovementController()
        {
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;
        }

        public void SetCamera(Camera camera)
        {
            _mainCamera = camera;
        }

        public void Update()
        {
            JumpAndGravity();
            GroundedCheck();
            Move();
        }
        
        private void GroundedCheck()
        {
            Vector3 spherePosition = new Vector3(_transform.position.x, _transform.position.y - GroundedOffset, _transform.position.z);
            _isGrounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);

            OnGroundedStateChanged?.Invoke(_isGrounded);
        }
        
        private void Move()
        {
            float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;
            
            if (_input.move == Vector2.zero) 
                targetSpeed = 0.0f;
            
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;
            
            if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (_animationBlend < 0.01f) 
                _animationBlend = 0f;
            
            Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;
            
            if (_input.move != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(_transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);
                
                _transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }


            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
            
            _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
            

            OnAnimationBlendChanged?.Invoke(_animationBlend);
            OnInputMagnitudeChanged?.Invoke(inputMagnitude);
        }

        private void JumpAndGravity()
        {
            if (_isGrounded)
            {
                _fallTimeoutDelta = FallTimeout;
                
                OnJumpStateChanged?.Invoke(false);
                OnFreeFallStateChanged?.Invoke(false);
                
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }
                
                if (_input.jump && _jumpTimeoutDelta <= 0.0f)
                {
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
                    
                    OnJumpStateChanged?.Invoke(true);
                }
                
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                _jumpTimeoutDelta = JumpTimeout;
                
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    OnFreeFallStateChanged?.Invoke(true);
                }
                
                _input.jump = false;
            }
            
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }
    }
}