using Mirror;
using UnityEngine;

namespace Gameplay
{
    public class FallDamageBehaviour : NetworkBehaviour
    {
        [SerializeField] private Health _health;
    }
}