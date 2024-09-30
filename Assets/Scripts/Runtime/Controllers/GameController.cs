using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Runtime.Controllers
{
    public class GameController : MonoBehaviour
    {
        [Header("Grid Settings")]
        [SerializeField] private int _gridResolutionX;
        [SerializeField] private int _gridResolutionZ;

        [Header("Claim Settings")] 
        [SerializeField] private float _claimRadius; // Represents the claim radius in world units
        [SerializeField] private int _cellsAroundCenter; // Number of cells around the center to own

        [Header("Particle Effect Settings")] 
        [SerializeField] private GameObject _particlePrefab; // Custom particle effect prefab
        [SerializeField] private Color player1Color = Color.red;
        [SerializeField] private Color player2Color = Color.blue;

        [Header("Terrain Settings")] 
        [SerializeField] private int _terrainWidth;
        [SerializeField] private int _terrainHeight;

        private Vector3[,] _cellWorldPositions;
        private int[,] _ownershipGrid;
        private int _totalEnvironmentCells;
        private int _totalClaimedCells;
        private float _cellWidth;
        private float _cellHeight;
    
        public static event Action<float> OnProgressUpdate;

        public static GameController Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            InitializeGrid();
            CalculateCellPositions();
        }

        private void InitializeGrid()
        {
            _ownershipGrid = new int[_gridResolutionX, _gridResolutionZ];
            _totalEnvironmentCells = _gridResolutionX * _gridResolutionZ;
            _cellWorldPositions = new Vector3[_gridResolutionX, _gridResolutionZ];
            _cellWidth = (float)_terrainWidth / _gridResolutionX;
            _cellHeight = (float)_terrainHeight / _gridResolutionZ;
        }

        // Calculate and store cell world positions
        private void CalculateCellPositions()
        {
            float cellWidth = (float)_terrainWidth / _gridResolutionX;
            float cellHeight = (float)_terrainHeight / _gridResolutionZ;

            float offsetX = (_gridResolutionX * cellWidth) / 2f;
            float offsetZ = (_gridResolutionZ * cellHeight) / 2f;

            for (int x = 0; x < _gridResolutionX; x++)
            {
                for (int z = 0; z < _gridResolutionZ; z++)
                {
                    _cellWorldPositions[x, z] = new Vector3(
                        x * cellWidth - offsetX + cellWidth / 2f,
                        0f,
                        z * cellHeight - offsetZ + cellHeight / 2f
                    );
                }
            }
        }

        // Get world position of a specific cell
        private Vector3 GetWorldPositionForCell(int x, int z)
        {
            return _cellWorldPositions[x, z];
        }

        public void ClaimTerrain(Vector3 centerPoint, int playerID)
        {
            Vector2Int centerCell = GetGridCell(centerPoint);

            List<Vector2Int> claimedCells = new List<Vector2Int>();

            int radiusX = _cellsAroundCenter > 0 ? _cellsAroundCenter : Mathf.CeilToInt(_claimRadius / _cellWidth);
            int radiusZ = _cellsAroundCenter > 0 ? _cellsAroundCenter : Mathf.CeilToInt(_claimRadius / _cellHeight);

            for (int x = Mathf.Max(centerCell.x - radiusX, 0);
                 x <= Mathf.Min(centerCell.x + radiusX, _gridResolutionX - 1);
                 x++)
            {
                for (int z = Mathf.Max(centerCell.y - radiusZ, 0);
                     z <= Mathf.Min(centerCell.y + radiusZ, _gridResolutionZ - 1);
                     z++)
                {
                    var cellWorldPosition = GetWorldPositionForCell(x, z);
                    var distance = Vector3.Distance(centerPoint, cellWorldPosition);

                    if (distance <= _claimRadius || _cellsAroundCenter > 0)
                    {
                        if (_ownershipGrid[x, z] == 0)
                        {
                            _ownershipGrid[x, z] = playerID;
                            _totalClaimedCells++;
                            claimedCells.Add(new Vector2Int(x, z));
                        }
                    }
                }
            }

            if (claimedCells.Count > 0)
            {
                CreateCombinedParticleSystem(claimedCells, playerID);
            }

            CheckWinCondition(1);
        }

        // Create a particle system that covers the boundaries of the owned cells and place it at the center
        private void CreateCombinedParticleSystem(List<Vector2Int> claimedCells, int playerID)
        {
            if (claimedCells.Count == 0) return;

            float cellWidth = (float)_terrainWidth / _gridResolutionX;
            float cellHeight = (float)_terrainHeight / _gridResolutionZ;

            int minX = claimedCells.Min(cell => cell.x);
            int maxX = claimedCells.Max(cell => cell.x);
            int minZ = claimedCells.Min(cell => cell.y);
            int maxZ = claimedCells.Max(cell => cell.y);

            float areaWidth = (maxX - minX + 1) * cellWidth;
            float areaHeight = (maxZ - minZ + 1) * cellHeight;

            Vector3 areaCenterPosition = GetWorldPositionForCell((minX + maxX) / 2, (minZ + maxZ) / 2);
            areaCenterPosition = new Vector3(areaCenterPosition.x, 3f, areaCenterPosition.z);

            GameObject areaParticle = Instantiate(_particlePrefab, areaCenterPosition, Quaternion.identity);
            ParticleSystem particleSystem = areaParticle.GetComponent<ParticleSystem>();

            if (particleSystem != null)
            {
                var mainModule = particleSystem.main;
                mainModule.startSizeX = areaWidth * 0.8f;
                mainModule.startSizeZ = areaHeight * 0.8f;
                mainModule.startSizeY = 0.1f;
                mainModule.startSizeY = 0f;

                var emissionModule = particleSystem.emission;
                emissionModule.rateOverTime = 25f;

                var shapeModule = particleSystem.shape;
                shapeModule.shapeType = ParticleSystemShapeType.Box;
                shapeModule.scale = new Vector3(areaWidth, 0f, areaHeight);

                mainModule.startLifetime = 3f;

                mainModule.startColor = playerID == 1 ? player1Color : player2Color;
            }
        }
    
        // Get the bounds of a specific cell
        public Bounds GetBoundsForCell(int x, int z)
        {
            float cellWidth = (float)_terrainWidth / _gridResolutionX;
            float cellHeight = (float)_terrainHeight / _gridResolutionZ;

            Vector3 cellCenter = GetWorldPositionForCell(x, z);
            Vector3 cellSize = new Vector3(cellWidth, 0f, cellHeight);

            return new Bounds(cellCenter, cellSize);
        }

        // Find the grid cell based on a world position
        public Vector2Int GetGridCell(Vector3 worldPosition)
        {
            float cellWidth = (float)_terrainWidth / _gridResolutionX;
            float cellHeight = (float)_terrainHeight / _gridResolutionZ;

            float offsetX = (_gridResolutionX * cellWidth) / 2f;
            float offsetZ = (_gridResolutionZ * cellHeight) / 2f;

            int cellX = Mathf.FloorToInt((worldPosition.x + offsetX) / cellWidth);
            int cellZ = Mathf.FloorToInt((worldPosition.z + offsetZ) / cellHeight);

            cellX = Mathf.Clamp(cellX, 0, _gridResolutionX - 1);
            cellZ = Mathf.Clamp(cellZ, 0, _gridResolutionZ - 1);

            return new Vector2Int(cellX, cellZ);
        }

        public float CalculateOwnershipPercentage(int playerID)
        {
            if (_ownershipGrid == null) return 0f;
            int playerOwnedCells = _ownershipGrid.Cast<int>().Count(cell => cell == playerID);
            return ((float)playerOwnedCells / _totalEnvironmentCells) * 100f;
        }

        private void CheckWinCondition(int playerID)
        {
            float ownershipPercentage = CalculateOwnershipPercentage(playerID);
            OnProgressUpdate.Invoke(ownershipPercentage);
            Debug.Log("Player " + playerID + " owns " + ownershipPercentage + "% of the terrain.");

            if (ownershipPercentage >= 100f)
            {
                Debug.Log("Player " + playerID + " has won by owning 100% of the terrain!");
                // Trigger win logic
            }
        }

        private void OnDrawGizmos()
        {
            if (_cellWorldPositions == null) return;

            float cellWidth = (float)_terrainWidth / _gridResolutionX;
            float cellHeight = (float)_terrainHeight / _gridResolutionZ;

            for (int x = 0; x < _gridResolutionX; x++)
            {
                for (int z = 0; z < _gridResolutionZ; z++)
                {
                    Vector3 cellCenter = GetWorldPositionForCell(x, z);
                    Vector3 cellSize = new Vector3(cellWidth, 0.1f, cellHeight);

                    Gizmos.color = _ownershipGrid != null && _ownershipGrid[x, z] != 0
                        ? Color.Lerp(player1Color, player2Color, _ownershipGrid[x, z] / 2.0f)
                        : Color.white;

                    Gizmos.DrawWireCube(cellCenter, cellSize);
                }
            }
        }
    }
}