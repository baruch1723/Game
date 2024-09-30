using System.Collections.Generic;
using Runtime.Helpers;
using Runtime.Managers;
using Runtime.Models;
using UnityEngine;

namespace Runtime.Components
{
    public class EnvironmentGrid : MonoBehaviour
    {
        private GridCell[,] _grid;
        private Bounds _bounds;
        private Vector3 _size;
        private Vector3 _center;
        private float _tileSize;

        [SerializeField] private int _columns;
        [SerializeField] private int _rows;

        private void CreateGrid()
        {
            _grid = new GridCell[_rows, _columns];
            _bounds = RandomPointGenerator.CalculateEnvironmentBounds();
            _center = _bounds.center;
            _tileSize = _bounds.size.x / _columns; // Use columns for calculating tile width

            var rowStart = -_rows / 2;
            var columnStart = -_columns / 2;

            for (var row = 0; row < _rows; row++)
            {
                for (var column = 0; column < _columns; column++)
                {
                    var tilePosition = _center + new Vector3((column + columnStart) * _tileSize, 0,
                        (row + rowStart) * _tileSize);
                    tilePosition = new Vector3(tilePosition.x, 0f, tilePosition.z);

                    _grid[row, column] = new GridCell
                    {
                        Row = row,
                        Column = column,
                        Position = tilePosition,
                        Owner = null
                    };

                    Debug.DrawLine(tilePosition, tilePosition + Vector3.right * _tileSize, Color.green, 100f);
                    Debug.DrawLine(tilePosition, tilePosition + Vector3.forward * _tileSize, Color.green, 100f);
                }
            }
        }

        public List<GridCell> GetCellsInRadius(Vector3 position, float radius)
        {
            var cellsInRadius = new List<GridCell>();

            for (int row = 0; row < _rows; row++)
            {
                for (int column = 0; column < _columns; column++)
                {
                    var cell = _grid[row, column];
                    var distance = Vector3.Distance(position, cell.Position);

                    if (distance <= radius)
                    {
                        cellsInRadius.Add(cell);
                    }
                }
            }

            return cellsInRadius;
        }

        public void SetNearbyCells()
        {
            var player = LevelManager.Instance.GetPlayer().gameObject;
            var cells = GetCellsInRadius(player.transform.position, 5f);
            foreach (var cell in cells)
            {
                AssignTileToPlayer(player, cell.Row, cell.Column);
            }
        }

        public Vector2Int GetCellFromPosition(Vector3 position)
        {
            var row = Mathf.FloorToInt((position.z - _center.z) / _tileSize + (_rows / 2));
            var column = Mathf.FloorToInt((position.x - _center.x) / _tileSize + (_columns / 2));

            row = Mathf.Clamp(row, 0, _rows - 1);
            column = Mathf.Clamp(column, 0, _columns - 1);

            return new Vector2Int(row, column);
        }

        public void AssignTileToPlayer(GameObject player, int row, int column)
        {
            if (_grid[row, column].Owner == player) return;

            _grid[row, column].Owner = player;
        }

        public GameObject GetTileOwner(int row, int column)
        {
            return _grid[row, column].Owner;
        }
    }
}