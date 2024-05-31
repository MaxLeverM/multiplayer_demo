using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace Gameplay
{
    public class AidKitSpawner : NetworkBehaviour
    {
        [SerializeField] private List<Transform> _spawnPoints;
        [SerializeField] private AidKit _aidKitPrefab;
        [SerializeField] private int _aidKitMaxCount = 10;

        private void Start()
        {
            if(!isServer)
                return;
            
            SpawnAidKits();
        }

        [Server]
        private void SpawnAidKits()
        {
            var randPoints = _spawnPoints.RandomRange(_aidKitMaxCount);

            foreach (var point in randPoints)
            {
                SpawnAidKit(point);
            }
        }

        [Server]
        private void SpawnAidKit(Transform spawnPoint)
        {
            var aidKit = Instantiate(_aidKitPrefab, spawnPoint.position, Quaternion.identity);
            NetworkServer.Spawn(aidKit.gameObject);
        }
    }
}