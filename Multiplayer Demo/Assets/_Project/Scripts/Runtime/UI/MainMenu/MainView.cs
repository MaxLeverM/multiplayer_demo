using System.Threading;
using _Project.Code.Runtime.Utils;
using _Project.Code.Runtime.ViewContracts;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI.MainMenu
{
    [AddressableView("MainView")]
    public class MainView : BaseView
    {
        [SerializeField] private Button _clientBtn;
        [SerializeField] private Button _hostBtn;
        [SerializeField] private TMP_InputField _nicknameInput;

        private MainViewModel _mainViewModel;
        
        protected override UniTask Init(IViewModel viewModel, CancellationToken ct)
        {
            _mainViewModel = (MainViewModel)viewModel;
            
            _clientBtn.onClick.AddListener(_mainViewModel.StartClient);
            _hostBtn.onClick.AddListener(_mainViewModel.StartHost);
            _nicknameInput.onValueChanged.AddListener(_mainViewModel.SetNickName);
            
            return UniTask.CompletedTask;
        }
    }
}