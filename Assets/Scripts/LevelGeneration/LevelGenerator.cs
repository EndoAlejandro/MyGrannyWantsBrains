using System;
using System.Collections.Generic;
using DarkHavoc.CustomUtils;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace LevelGeneration
{
    public class LevelGenerator : Singleton<LevelGenerator>
    {
        [SerializeField] private Vector2Int roomSize;
        [SerializeField] private Vector2Int levelSize;

        [SerializeField] private Transform globalRooms;
        [SerializeField] private Transform prefabRoomsPool;

        private NavMeshSurface _navMesh;
        
        private GridRoom[] _prefabGridRooms;
        private GridRoomData[,] _roomDataMatrix;

        private readonly Dictionary<string, Tilemap> _globalTilemaps = new();

        public GridRoomData InitialRoom { get; private set; }
        public GridRoomData ExitRoom { get; private set; }

        public List<Vector3> WorldPositionSpawnPoints { get; private set; }
        public List<Instantiable> Instantiables { get; private set; }

        protected override void SingletonAwake()
        {
            _navMesh = GetComponentInChildren<NavMeshSurface>();
            
            _roomDataMatrix = new GridRoomData[levelSize.x, levelSize.y + 2];

            // Non-Tile objects
            WorldPositionSpawnPoints = new List<Vector3>();
            Instantiables = new List<Instantiable>();

            // Load Room Prefabs
            _prefabGridRooms = prefabRoomsPool.GetComponentsInChildren<GridRoom>(true);

            // Fill global Tilemaps
            Tilemap[] tiles = globalRooms.GetComponentsInChildren<Tilemap>();
            foreach (var tilemap in tiles) _globalTilemaps.Add(tilemap.gameObject.tag, tilemap);
        }

        [ContextMenu("Generate Level")]
        public void GenerateLevel()
        {
            SetRoomsPrefabsState(true);

            CalculateMainPath();
            CalculateSecondaryPath();
            InstantiateRooms();

            SetRoomsPrefabsState(false);
            
            _navMesh.RemoveData();
            _navMesh.BuildNavMesh();
        }

        private void SetRoomsPrefabsState(bool state)
        {
            foreach (var room in _prefabGridRooms) room.gameObject.SetActive(state);
        }

        private void InstantiateRooms()
        {
            for (int i = 0; i < _roomDataMatrix.GetLength(0); i++)
            for (int j = 0; j < _roomDataMatrix.GetLength(1); j++)
            {
                var dataRoom = _roomDataMatrix[i, j];
                if (dataRoom == null) continue;

                RoomVariant selectedPrefabRoomVariant = SelectPrefabGridRoom(dataRoom);
                var offsetPosition = new Vector3Int(roomSize.x * i, 0, roomSize.y * j);
                Instantiate(selectedPrefabRoomVariant, offsetPosition, Quaternion.identity, globalRooms);
            }
        }

        private RoomVariant SelectPrefabGridRoom(GridRoomData dataRoom)
        {
            GridRoom selectedPrefabGridRoomVariant = _prefabGridRooms[0];

            foreach (var gridRoom in _prefabGridRooms)
            {
                if (gridRoom.Directions != dataRoom.Directions) continue;
                selectedPrefabGridRoomVariant = gridRoom;
                break;
            }

            return selectedPrefabGridRoomVariant.GetRandomVariant();
        }

        // TODO: Check if add instantiables is needed.
        private void AddInstantiables(RoomVariant prefabRoomVariant, int x, int y)
        {
            Transform[] localInstantiables = prefabRoomVariant.GetInstantiables();

            foreach (var instantiable in localInstantiables)
            {
                Vector3 localPosition = instantiable.position;
                Vector3 offsetPosition = new Vector3(localPosition.x + roomSize.x * x, localPosition.y - roomSize.y * y,
                    localPosition.z);
                Instantiables.Add(new Instantiable(instantiable, offsetPosition));
            }
        }

        private void CalculateMainPath()
        {
            var initialX = Random.Range(0, _roomDataMatrix.GetLength(0));
            SetInitialRoom(initialX);

            var nextRoom = new GridRoomData(new Vector2Int(initialX, 1));
            nextRoom.SetDirection(Vector2Int.down);
            _roomDataMatrix[initialX, 1] = nextRoom;

            Vector2Int currentPosition = nextRoom.Position;

            int safeExit = 100;
            while (safeExit > 0)
            {
                safeExit--;

                var currentRoom = _roomDataMatrix[currentPosition.x, currentPosition.y];
                var direction = GetNextDirection(currentRoom);

                if (direction == Vector2Int.zero)
                {
                    currentRoom.SetDirection(Vector2Int.up);
                    break;
                }

                currentRoom.SetDirection(direction);

                var customPosition = currentRoom.Position;
                var room = new GridRoomData(customPosition + direction);

                room.SetDirection(direction * -1);
                _roomDataMatrix[room.Position.x, room.Position.y] = room;
                currentPosition = room.Position;
            }

            currentPosition += Vector2Int.up;
            SetExitRoom(currentPosition);
        }

        private void SetExitRoom(Vector2Int currentPosition)
        {
            ExitRoom = new GridRoomData(currentPosition);
            ExitRoom.SetDirection(Vector2Int.down);
            _roomDataMatrix[currentPosition.x, currentPosition.y] = ExitRoom;
        }

        private void SetInitialRoom(int initialX)
        {
            InitialRoom = new GridRoomData(new Vector2Int(initialX, 0));
            InitialRoom.SetDirection(Vector2Int.up);
            _roomDataMatrix[initialX, 0] = InitialRoom;
        }

        private void CalculateSecondaryPath()
        {
            for (int i = 0; i < _roomDataMatrix.GetLength(0); i++)
            for (int j = 1; j < _roomDataMatrix.GetLength(1) - 1; j++)
            {
                var currentPosition = _roomDataMatrix[i, j];
                if (currentPosition != null) continue;
                if (Random.Range(0f, 1f) < .25f) continue;

                if (i > 0)
                {
                    if (TryToAddSecondaryRoom(i, j, -1)) continue;
                }

                if (i < _roomDataMatrix.GetLength(0) - 1)
                {
                    if (TryToAddSecondaryRoom(i, j, 1)) continue;
                }
            }
        }

        private bool TryToAddSecondaryRoom(int i, int j, int direction)
        {
            var neighbour = _roomDataMatrix[i + direction, j];
            if (neighbour == null) return false;

            var newRoom = new GridRoomData(new Vector2Int(i, j));
            newRoom.SetDirection(Vector2Int.right * direction);
            neighbour.SetDirection(Vector2Int.right * -direction);

            _roomDataMatrix[i, j] = newRoom;
            return true;
        }

        private Vector2Int GetNextDirection(GridRoomData sourceRoom)
        {
            bool rightCheck = sourceRoom.Directions.y == 0 &&
                              sourceRoom.Position.x < _roomDataMatrix.GetLength(0) - 1;
            bool bottomCheck = sourceRoom.Directions.x == 0 &&
                               sourceRoom.Position.y < _roomDataMatrix.GetLength(1) - 2;
            bool leftCheck = sourceRoom.Directions.w == 0 &&
                             sourceRoom.Position.x > 0;

            if (rightCheck & !bottomCheck & !leftCheck) return Vector2Int.right;
            if (!rightCheck & !bottomCheck & leftCheck) return Vector2Int.left;
            if (!rightCheck & bottomCheck & !leftCheck) return Vector2Int.up;
            if (rightCheck & !bottomCheck & leftCheck)
                return Random.Range(0f, 1f) > .5f ? Vector2Int.right : Vector2Int.left;
            if (rightCheck & bottomCheck & !leftCheck)
                return Random.Range(0f, 1f) > .5f ? Vector2Int.up : Vector2Int.right;
            if (!rightCheck & bottomCheck & leftCheck)
                return Random.Range(0f, 1f) > .5f ? Vector2Int.up : Vector2Int.left;

            if (rightCheck & bottomCheck & leftCheck)
            {
                float prob = Random.Range(0f, 1f);
                return prob switch
                {
                    > .66f => Vector2Int.right,
                    > .33f => Vector2Int.left,
                    _ => Vector2Int.up
                };
            }

            return Vector2Int.zero;
        }

        public Vector3 GetWorldPosition(GridRoomData data) =>
            new(roomSize.x * data.Position.x, 0f, roomSize.y * -data.Position.y);
    }
}