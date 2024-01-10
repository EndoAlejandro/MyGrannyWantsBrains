using UnityEngine;

namespace LevelGeneration
{
    public class GridRoom : MonoBehaviour
    {
        [SerializeField] private Vector4 directions;

        private RoomVariant[] _variants;
        public Vector4 Directions => directions;

        private void Awake() => _variants = GetComponentsInChildren<RoomVariant>(true);

        public RoomVariant GetRandomVariant()
        {
            _variants = GetComponentsInChildren<RoomVariant>(true);
            int index = Random.Range(0, _variants.Length);
            return GetVariant(index);
        }

        private RoomVariant GetVariant(int index) => _variants[index];
    }
}