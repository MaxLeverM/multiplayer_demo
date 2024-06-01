using UnityEngine;

namespace Gameplay
{
    public interface ISpawnPositions
    {
        Transform TakePosition();
        void ReturnPosition();
    }
}