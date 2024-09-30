using Runtime.Interfaces;
using UnityEngine;

namespace Runtime.Factories
{
    public class EnemyFactory : ISpawnFactory
    {
        private readonly GameObject _enemyPrefab;

        public EnemyFactory(GameObject enemyPrefab)
        {
            _enemyPrefab = enemyPrefab;
        }

        public GameObject Create(Vector3 position)
        {
            var enemy = Object.Instantiate(_enemyPrefab);
            enemy.transform.position = position;
            return enemy;
        }
    }
}