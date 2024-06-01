using Cinemachine;
using UnityEngine;

namespace Gameplay
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CinemachineCameraController : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;

        public void SetTarget(Transform target)
        {
            _virtualCamera.Follow = target;
        }
    }
}