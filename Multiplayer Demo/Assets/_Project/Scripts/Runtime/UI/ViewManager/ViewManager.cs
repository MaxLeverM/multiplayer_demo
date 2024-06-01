using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using _Project.Code.Runtime.Utils;
using _Project.Code.Runtime.ViewContracts;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace _Project.Code.Runtime.ViewManager
{
    public class ViewManager : IViewManager
    {
        private readonly RectTransform _parentLayer;
        private Dictionary<Type, BaseView> _openedViews;

        public ViewManager(RectTransform parentLayer)
        {
            _parentLayer = parentLayer;
            _openedViews = new Dictionary<Type, BaseView>();
        }

        public async UniTask ShowAsync<TView>(IViewModel viewModel, CancellationToken ct = default) where TView : BaseView
        {
            var attr = typeof(TView).GetCustomAttribute<AddressableViewAttribute>();
            if (attr == null)
            {
                throw new InvalidOperationException($"Type {typeof(TView).Name} must contain {nameof(AddressableViewAttribute)} attribute");
            }
            var keyName = attr.KeyName;
            
            var prefab = await Addressables.LoadAssetAsync<GameObject>(keyName).Task;
            if (prefab.TryGetComponent<TView>(out var prefabView))
            {
                var view = Object.Instantiate(prefabView, _parentLayer);
                view.gameObject.SetActive(true);
                await view.ShowAsync(viewModel, ct);
                _openedViews.Add(typeof(TView), view);
            }
            else
            {
                Debug.LogError($"Prefab for key {keyName} must contain {nameof(TView)} component!");
            }
        }

        public async UniTask HideAsync<TView>(CancellationToken ct = default) where TView : BaseView
        {
            var viewType = typeof(TView);
            if (_openedViews.TryGetValue(viewType, out var view))
            {
                await view.HideAsync(ct);
                _openedViews.Remove(viewType);
                Object.Destroy(view.gameObject);
            }
            else
            {
                Debug.LogError($"Can't hide view {viewType.Name}, it wasn't open!");
            }
        }
    }
}