using UnityEngine;
using Zenject;

namespace Gameplay.Installers
{
    public class InputInstaller : MonoInstaller
    {
        [SerializeField] private InputController _inputControllerPrefab;
        [SerializeField] private CinemachineCameraController _cameraControllerPrefab;
        [SerializeField] private GameObject _playerPrefab;
        public const string PlayerPrefabID = "PlayerPrefab";
        public override void InstallBindings()
        {
            Container.Bind<InputController>().FromComponentInNewPrefab(_inputControllerPrefab).AsSingle().NonLazy();
            
            Container.Bind<GameObject>().WithId(PlayerPrefabID).FromInstance(_playerPrefab);
            Container.BindFactory<Player, Player.Factory>().FromComponentInNewPrefab(_playerPrefab);
            Container.Bind<PlayerSpawner>().AsSingle().NonLazy();

            Container.Bind<CinemachineCameraController>().FromComponentInNewPrefab(_cameraControllerPrefab).AsSingle();
        }
    }
}