using System;

namespace Gameplay
{
    public interface IDeathable<out T>
    {
        event Action<T> OnDeath;
    }
}