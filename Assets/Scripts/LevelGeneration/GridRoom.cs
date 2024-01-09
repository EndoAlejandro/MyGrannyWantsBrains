using UnityEngine;

namespace LevelGeneration
{
    public class GridRoom : MonoBehaviour
    {
        [SerializeField] private Vector4 directions;

        private GridRoomVariant[] _variants;
        public Vector4 Directions => directions;

        private void Awake() => _variants = GetComponentsInChildren<GridRoomVariant>(true);

        public GridRoomVariant GetRandomVariant()
        {
            _variants = GetComponentsInChildren<GridRoomVariant>(true);
            int index = Random.Range(0, _variants.Length);
            return GetVariant(index);
        }

        private GridRoomVariant GetVariant(int index) => _variants[index];
    }
}