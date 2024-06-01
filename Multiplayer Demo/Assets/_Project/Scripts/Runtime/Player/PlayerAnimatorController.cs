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

        [SerializeField] private string _animSpeed = "Speed";
        [SerializeField] private string _animGrounded = "Grounded";
        [SerializeField] private string _animJump = "Jump";
        [SerializeField] private string _animFreeFall = "FreeFall";
        [SerializeField] private string _animMotionSpeed = "MotionSpeed";
        
        private int _animIDSpeed;
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

        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash(_animSpeed);
            _animIDGrounded = Animator.StringToHash(_animGrounded);
            _animIDJump = Animator.StringToHash(_animJump);
            _animIDFreeFall = Animator.StringToHash(_animFreeFall);
            _animIDMotionSpeed = Animator.StringToHash(_animMotionSpeed);
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