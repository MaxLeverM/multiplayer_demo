using UnityEngine;

namespace Gameplay
{
    public interface IControllable
    {
        void Move(Vector2 newMoveDirection);
        void Jump(bool newJumpState);
        void Sprint(bool newSprintState);
        void Look(Vector2 newLookDirection);
    }
}