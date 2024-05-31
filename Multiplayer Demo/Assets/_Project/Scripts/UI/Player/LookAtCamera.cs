using System;
using UnityEngine;

namespace Gameplay.UI.Player
{
    public class LookAtCamera : MonoBehaviour
    {
        private Transform _mainCamera;
        private void Start()
        {
            _mainCamera = Camera.main.transform;
        }

        private void Update()
        {
            transform.rotation = Quaternion.LookRotation(transform.position - _mainCamera.position);
        }
    }
}