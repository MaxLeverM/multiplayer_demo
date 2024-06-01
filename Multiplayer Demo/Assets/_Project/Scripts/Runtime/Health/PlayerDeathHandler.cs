namespace Gameplay
{
    public class PlayerDeathHandler : IDeathHandler<Health>
    {
        private readonly ISpawnPositions _playerSpawnPositions;

        public PlayerDeathHandler(PlayerSpawnPositions playerSpawnPositions)
        {
            _playerSpawnPositions = playerSpawnPositions;
        }

        public void AddObject(IDeathable<Health> deathObject)
        {
            deathObject.OnDeath += Death;
        }

        private void Death(Health health)
        {
            health.RestoreHealth(health.MaxHealth);

            var point = _playerSpawnPositions.TakePosition();
            if (health.TryGetComponent(out MovementController movementController))
            {
                movementController.TeleportRPC(point.position, point.rotation);
            }
        }
    }
}