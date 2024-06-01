using Mirror;
using UnityEngine;
using Zenject;

namespace Gameplay
{
    public class Player : NetworkBehaviour
    {
        [SerializeField] private PlayerInputBehaviour _inputBehaviour;
        [SerializeField] private PlayerCameraController _playerCamera;
        private InputController _inputController;
        private CinemachineCameraController _cameraController;

        [Inject]
        private void Construct(InputController inputController, CinemachineCameraController cameraController)
        {
            _inputController = inputController;
            _cameraController = cameraController;
        }

        public override void OnStartLocalPlayer()
        {
            _inputController.SetTarget(_inputBehaviour);
            _cameraController.SetTarget(_playerCamera.CameraTarget);
        }

        public class Factory : PlaceholderFactory<Player>
        {
        }
    }
}