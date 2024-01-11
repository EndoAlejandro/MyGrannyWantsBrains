using UnityEngine;

namespace Pooling
{
    public sealed class PoolAfterSeconds : PooledMonoBehaviour
    {
        [SerializeField] private float delay;
        private ParticleSystem _particleSystem;

        private void OnEnable()
        {
            _particleSystem ??= GetComponent<ParticleSystem>();
            if (_particleSystem == null) _particleSystem = GetComponentInChildren<ParticleSystem>();
            _particleSystem.Stop();
            _particleSystem.Play();
            ReturnToPool(delay);
        }
    }
}