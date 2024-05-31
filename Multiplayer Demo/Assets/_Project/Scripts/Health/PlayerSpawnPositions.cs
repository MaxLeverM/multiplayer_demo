using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class PlayerSpawnPositions : MonoBehaviour, ISpawnPositions
    {
        [SerializeField] private List<Transform> _spawnPoints;
        public Transform TakePosition()
        {
            return _spawnPoints.Random();
        }

        public void ReturnPosition()
        {
        }
    }
}