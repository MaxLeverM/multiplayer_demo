using UnityEngine;

namespace Gameplay
{
    public class PlayerCameraController : MonoBehaviour
    {
        [SerializeField] private Transform _cameraTarget;

        [Tooltip("How far in degrees can you move the camera up")]
        public float TopClamp = 70.0f;

        [Tooltip("How far in degrees can you move the camera down")]
        public float BottomClamp = -30.0f;

        [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
        public float CameraAngleOverride = 0.0f;

        [Tooltip("For locking the camera position on all axis")]
        public bool LockCameraPosition = false;
        
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;
        
        private const float _threshold = 0.01f;

        public Transform CameraTarget => _cameraTarget;
        
        public Vector2 LookDirection { get; set; }
        private void Awake()
        {
            _cinemachineTargetYaw = _cameraTarget.transform.rotation.eulerAngles.y;
        }

        private void LateUpdate()
        {
            CameraRotation();
        }

        private void CameraRotation()
        {
            if (LookDirection.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                _cinemachineTargetYaw += LookDirection.x;
                _cinemachineTargetPitch += LookDirection.y;
            }
            
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);
            
            _cameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
        }
        
        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }
    }
}