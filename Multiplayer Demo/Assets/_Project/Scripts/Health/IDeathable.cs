using System;

namespace Gameplay
{
    public interface IDeathable<out T>
    {
        public event Action<T> OnDeath;
    }
}