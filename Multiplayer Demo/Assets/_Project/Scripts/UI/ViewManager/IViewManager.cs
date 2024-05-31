using System.Threading;
using _Project.Code.Runtime.ViewContracts;
using Cysharp.Threading.Tasks;

namespace _Project.Code.Runtime.ViewManager
{
    public interface IViewManager
    {
        public UniTask ShowAsync<TView>(IViewModel viewModel, CancellationToken ct = default) where TView : BaseView;
        public UniTask HideAsync<TView>(CancellationToken ct) where TView : BaseView;
    }
}