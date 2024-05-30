using System;
using Mirror;
using UnityEngine;

namespace Gameplay
{
    [RequireComponent(typeof(CharacterController))]
    public class MovementController : NetworkBehaviour
    {
        [Tooltip("Move speed of the character in m/s")]
        [SerializeField] public float MoveSpeed = 2.0f;
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
        
        [SerializeField] private CharacterController _controller;
        private Camera _mainCamera;

        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;
        [SyncVar] private bool _isGrounded = true;
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;
        
        public float GetGroundedOffset => GroundedOffset;

        public float GetGroundedRadius => GroundedRadius;

        public bool IsGrounded => _isGrounded;
        
        public bool InputJump { get; set; }
        public bool InputSprint { get; set; }
        public Vector2 InputMove { get; set; }

        private void Awake()
        {
            _mainCamera = Camera.main;
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;
        }

        private void Update()
        {
            if(!isLocalPlayer)
                return;
            
            JumpAndGravity();
            GroundedCheck();
            Move();
        }
        
        private void GroundedCheck()
        {
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
            _isGrounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);

            OnGroundedStateChanged?.Invoke(_isGrounded);
        }
        
        private void Move()
        {
            float targetSpeed = InputSprint ? SprintSpeed : MoveSpeed;
            
            if (InputMove == Vector2.zero) 
                targetSpeed = 0.0f;
            
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = 1f;
            
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
            
            Vector3 inputDirection = new Vector3(InputMove.x, 0.0f, InputMove.y).normalized;
            
            if (InputMove != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);
                
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
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
                
                if (InputJump && _jumpTimeoutDelta <= 0.0f)
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
                
                InputJump = false;
            }
            
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }
    }
}