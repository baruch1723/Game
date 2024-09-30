using Runtime.Interfaces;
using UnityEngine;

namespace Runtime.Factories
{
    public class TreasureFactory : ISpawnFactory
    {
        private readonly GameObject _treasurePrefab;

        public TreasureFactory(GameObject treasurePrefab)
        {
            _treasurePrefab = treasurePrefab;
        }

        public GameObject Create(Vector3 position)
        {
            var treasure = Object.Instantiate(_treasurePrefab);
            treasure.transform.position = position;
            return treasure;
        }
    }
}