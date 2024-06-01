using _Project.Code.Runtime.ViewManager;
using Gameplay.UI;
using Gameplay.UI.MainMenu;
using UnityEngine;
using Zenject;

namespace Gameplay.Installers
{
    public class MenuInstaller : MonoInstaller
    {
        [SerializeField] private RectTransform _parentCanvas;
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<MenuInitializer>().AsSingle();
            Container.BindInterfacesAndSelfTo<ViewManager>().FromInstance(new ViewManager(_parentCanvas)).AsSingle();
            Container.BindInterfacesAndSelfTo<MainViewModel>().AsSingle();
        }
    }
}