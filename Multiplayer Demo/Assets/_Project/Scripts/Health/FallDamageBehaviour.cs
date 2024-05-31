using Mirror;
using UnityEngine;

namespace Gameplay
{
    public class FallDamageBehaviour : NetworkBehaviour
    {
        [SerializeField] private Health _health;
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private MovementController _movementController;
        [SerializeField] private float _fallVelocityThreshold = -10f;
        [SerializeField] private AnimationCurve _fallDamageProgression;

        private void Start()
        {
            _movementController.OnGroundedStateChanged += GroundedStateChanged;
        }

        private void GroundedStateChanged(bool isGrounded)
        {
            if(!isGrounded || !isLocalPlayer)
                return;

            var fallVelocity = _characterController.velocity.y;
            if (fallVelocity < _fallVelocityThreshold)
            {
                int damage = (int)_fallDamageProgression.Evaluate(Mathf.Abs(fallVelocity));
                _health.CmdTakeHealth(damage);
            }
        }
    }
}