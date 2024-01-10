using CustomUtils;
using UnityEngine;

namespace LevelGeneration
{
    public class RoomVariant : MonoBehaviour
    {
        /*[SerializeField]*/ private Transform spawnPointsContainer;
        /*[SerializeField]*/ private Transform instantiables;
        public Transform[] GetSpawnPoints() => spawnPointsContainer.GetChildren();
        public Transform[] GetInstantiables() => instantiables.GetChildren();
    }
}