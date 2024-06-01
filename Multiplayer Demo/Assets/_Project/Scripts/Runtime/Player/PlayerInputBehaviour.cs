using Mirror;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay
{
    public class PlayerInputBehaviour : NetworkBehaviour, IControllable
    {
        [FormerlySerializedAs("_cameraController")] [SerializeField] private PlayerCameraController _playerCameraController;
        [SerializeField] private MovementController _movementController;

        public void Move(Vector2 newMoveDirection)
        {
            _movementController.InputMove = newMoveDirection;
        }

        public void Jump(bool newJumpState)
        {
            _movementController.InputJump = newJumpState;
        }

        public void Sprint(bool newSprintState)
        {
            _movementController.InputSprint = newSprintState;
        }

        public void Look(Vector2 newLookDirection)
        {
            _playerCameraController.LookDirection = newLookDirection;
        }
    }
}