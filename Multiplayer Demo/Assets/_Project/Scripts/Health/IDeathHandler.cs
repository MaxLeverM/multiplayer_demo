namespace Gameplay
{
    public interface IDeathHandler<in T>
    {
        void AddObject(IDeathable<T> deathObject);
    }
}