using Mirror;
using UnityEngine;

namespace Gameplay
{
    public class AidKit : NetworkBehaviour
    {
        [SerializeField][SyncVar] private int _restorePoints = 100;
        private void OnTriggerEnter(Collider other)
        {
            if(!isServer)
                return;
            if(!other.CompareTag("Player"))
                return;

            if (other.TryGetComponent(out Health health))
            {
                if (health.HealthPoints.Value < health.MaxHealth)
                {
                    health.RestoreHealth(_restorePoints);
                    NetworkServer.Destroy(gameObject);
                }
            }
        }
    }
}