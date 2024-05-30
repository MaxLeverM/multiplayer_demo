using System;
using Mirror;
using UnityEngine;

namespace Gameplay
{
    [Serializable]
    public class PlayerAnimatorController : NetworkBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private MovementController _movementController;

        private int _animIDSpeed; //рефакторинг
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall;
        private int _animIDMotionSpeed;

        private void Awake()
        {
            AssignAnimationIDs();
            SubscribeOnMovement();
            
            SetGrounded(_movementController.IsGrounded);
        }

        private void SubscribeOnMovement()
        {
            _movementController.OnGroundedStateChanged += SetGrounded;
            _movementController.OnAnimationBlendChanged += SetAnimationBlend;
            _movementController.OnInputMagnitudeChanged += SetInputMagnitude;
            _movementController.OnJumpStateChanged += SetJump;
            _movementController.OnFreeFallStateChanged += SetFreeFall;
        }

        private void SetFreeFall(bool isFreeFall)
        {
            _animator.SetBool(_animIDFreeFall, isFreeFall);
        }

        private void SetJump(bool isJump)
        {
            _animator.SetBool(_animIDJump, isJump);
        }

        private void SetInputMagnitude(float inputMagnitude)
        {
            _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
        }

        private void SetAnimationBlend(float animationBlend)
        {
            _animator.SetFloat(_animIDSpeed, animationBlend);
        }

        private void SetGrounded(bool isGrounded)
        {
            _animator.SetBool(_animIDGrounded, isGrounded);
        }
        
        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        }

        public void Dispose()
        {
            _movementController.OnGroundedStateChanged -= SetGrounded;
            _movementController.OnAnimationBlendChanged -= SetAnimationBlend;
            _movementController.OnInputMagnitudeChanged -= SetInputMagnitude;
            _movementController.OnJumpStateChanged -= SetJump;
            _movementController.OnFreeFallStateChanged -= SetFreeFall;
        }
    }
}