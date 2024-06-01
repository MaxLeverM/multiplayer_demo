using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Code.Runtime.ViewContracts
{
    public abstract class BaseView : MonoBehaviour
    {
        public async UniTask ShowAsync(IViewModel viewModel, CancellationToken ct)
        {
            await Init(viewModel, ct);
            gameObject.SetActive(true);
            await OnShow(ct);
        }

        public async UniTask HideAsync(CancellationToken ct)
        {
            await BeforeHide(ct);
            gameObject.SetActive(false);
            await AfterHide(ct);
        }

        protected abstract UniTask Init(IViewModel viewModel, CancellationToken ct);

        protected virtual UniTask OnShow(CancellationToken ct)
        {
            return UniTask.CompletedTask;
        }
        protected virtual UniTask BeforeHide(CancellationToken ct)
        {
            return UniTask.CompletedTask;
        }
        protected virtual UniTask AfterHide(CancellationToken ct)
        {
            return UniTask.CompletedTask;
        }
    }
}