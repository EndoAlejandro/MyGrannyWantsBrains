using UnityEngine;

namespace LevelGeneration
{
    public struct Instantiable
    {
        public Transform Prefab { get; }
        public Vector3 WorldPosition { get; }

        public Instantiable(Transform prefab, Vector3 worldPosition)
        {
            Prefab = prefab;
            WorldPosition = worldPosition;
        }
    }
}