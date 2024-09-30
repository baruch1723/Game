using System;
using System.Collections.Generic;
using Runtime.Factories;
using Runtime.Helpers;
using Runtime.Interfaces;
using UnityEngine;

namespace Runtime.Managers
{
    public class SpawnManager : MonoBehaviour
    {
        public GameObject TreasurePrefab;
        public GameObject PeeSpotPrefab;
        public GameObject GhostEnemyPrefab;
        public LayerMask ObstecleLayer;

        [SerializeField] private float _spreadRadius = 5f;
        [SerializeField] private float _avoidObstacleRadius = 1f;

        public List<GameObject> Enemies = new();
        public List<GameObject> Treasures = new();
        public List<GameObject> PeeSpots = new();

        public static SpawnManager Instance { get; private set; }

        private ISpawnFactory _enemyFactory;
        private ISpawnFactory _treasureFactory;
        private ISpawnFactory _peeSpotFactory;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeFactories();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            
        }

        private void InitializeFactories()
        {
            _enemyFactory = new EnemyFactory(GhostEnemyPrefab);
            _treasureFactory = new TreasureFactory(TreasurePrefab);
            _peeSpotFactory = new TreasureFactory(PeeSpotPrefab);
        }

        public void SpawnTreasure()
        {
            var randomPoint = RandomPointGenerator.GetRandomPoint(_spreadRadius, _avoidObstacleRadius, ObstecleLayer);
            var treasure = _treasureFactory.Create(randomPoint);
            Treasures.Add(treasure);
        }

        public void SpawnEnemy()
        {
            var randomPoint = RandomPointGenerator.GetRandomPoint(_spreadRadius, _avoidObstacleRadius, ObstecleLayer);
            var enemy = _enemyFactory.Create(randomPoint);
            Enemies.Add(enemy);
        }
        
        public GameObject SpawnPeeSpot()
        {
            var randomPoint = RandomPointGenerator.GetRandomPoint(_spreadRadius, _avoidObstacleRadius, ObstecleLayer);
            var peeSpot = _peeSpotFactory.Create(randomPoint);
            return peeSpot;
        }


        public void Test()
        {
            for (int i = 0; i < 11; i++)
            {
                //SpawnTreasure();
                SpawnPeeSpot();
                //SpawnEnemy();
            }
        }
    }
}