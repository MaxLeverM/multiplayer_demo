using UnityEngine;
using Zenject;

namespace Gameplay
{
    public class EntryPoint : MonoBehaviour
    {
        private void Awake()
        {
            var _ = ProjectContext.Instance;
        }
    }
}