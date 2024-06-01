using _Project.Code.Runtime.ViewManager;
using Gameplay.UI.MainMenu;
using Zenject;

namespace Gameplay.UI
{
    public class MenuInitializer : IInitializable
    {
        private readonly IViewManager _viewManager;
        private readonly MainViewModel _mainViewModel;

        public MenuInitializer(IViewManager viewManager, MainViewModel mainViewModel)
        {
            _viewManager = viewManager;
            _mainViewModel = mainViewModel;
        }

        public async void Initialize()
        {
            await _viewManager.ShowAsync<MainView>(_mainViewModel);
        }
    }
}