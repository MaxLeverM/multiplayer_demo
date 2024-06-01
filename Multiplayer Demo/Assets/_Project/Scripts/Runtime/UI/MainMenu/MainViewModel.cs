using _Project.Code.Runtime.ViewContracts;
using Gameplay.Multiplayer;

namespace Gameplay.UI.MainMenu
{
    public class MainViewModel : IViewModel
    {
        private readonly GameNetworkManager _gameNetworkManager;

        public MainViewModel(GameNetworkManager gameNetworkManager)
        {
            _gameNetworkManager = gameNetworkManager;
        }

        public void StartClient()
        {
            _gameNetworkManager.StartClient();
        }

        public void StartHost()
        {
            _gameNetworkManager.StartHost();
        }

        public void SetNickName(string nickName)
        {
            _gameNetworkManager.NickName = nickName;
        }
    }
}