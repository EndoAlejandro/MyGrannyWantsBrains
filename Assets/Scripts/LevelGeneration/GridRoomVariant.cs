using DarkHavoc.CustomUtils;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LevelGeneration
{
    public class GridRoomVariant : MonoBehaviour
    {
        /*[SerializeField]*/ private Transform spawnPointsContainer;
        /*[SerializeField]*/ private Transform instantiables;

        private Tilemap[] _roomTileMaps;

        public Tilemap[] GetTileLayers()
        {
            if (_roomTileMaps == null || _roomTileMaps.Length == 0)
                _roomTileMaps = GetComponentsInChildren<Tilemap>(true);

            return _roomTileMaps;
        }

        public Transform[] GetSpawnPoints() => spawnPointsContainer.GetChildren();
        public Transform[] GetInstantiables() => instantiables.GetChildren();
    }
}