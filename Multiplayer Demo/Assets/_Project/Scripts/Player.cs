using System;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Gameplay
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInput))]
    public class Player : NetworkBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private CharacterController _controller;
        [SerializeField] private StarterAssetsInputs _input;
        [SerializeField] private GameObject _cinemachineCameraTarget;
        
        [SerializeField] private MovementController _movementController;
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private PlayerAnimatorController _animatorController;


        private void Awake()
        {
            _cameraController = new CameraController(_input, _cinemachineCameraTarget);
            _animatorController = new PlayerAnimatorController(_animator, _movementController);
            _movementController.SetCamera(Camera.main);
        }

        private void Update()
        {
            _movementController.Update();
        }

        private void LateUpdate()
        {
            _cameraController.Update();
        }

        private void OnDestroy()
        {
            _animatorController?.Dispose();
        }

     /*   private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (_movementController.IsGrounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - _movementController.GetGroundedOffset,
                    transform.position.z),
                _movementController.GetGroundedRadius);
        }*/
    }
}