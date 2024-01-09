using System;
using UnityEngine;

namespace LevelGeneration
{
    [Serializable]
    public class GridRoomData
    {
        private Vector4 _directions;
        public Vector2Int Position { get; private set; }
        public Vector4 Directions => _directions;

        public GridRoomData(Vector2Int position) => Position = position;
        // public void SetDirections(Vector4 directions) => _directions = directions;

        public void SetDirection(Vector2Int direction)
        {
            switch (direction)
            {
                case var _ when direction == Vector2Int.down:
                    _directions.x = 1;
                    break;

                case var _ when direction == Vector2Int.right:
                    _directions.y = 1;
                    break;
                case var _ when direction == Vector2Int.up:
                    _directions.z = 1;
                    break;
                case var _ when direction == Vector2Int.left:
                    _directions.w = 1;
                    break;
                default:
                    _directions = Vector4.zero;
                    break;
            }
        }
    }
}