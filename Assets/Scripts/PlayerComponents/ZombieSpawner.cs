using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class ZombieSpawner : MonoBehaviour
{
    [SerializeField] private Zombie zombiePrefab;

    [SerializeField] private float spawnDistance;
    [SerializeField] private float spawnTime;
    [SerializeField] private int spawnAmount;

    private PlayerController _player;

    private float _timer;

    private void Awake() => _player = GetComponent<PlayerController>();
    private void Start() => _timer = spawnTime;

    private void Update()
    {
        if (_timer > 0f) _timer -= Time.deltaTime;
        else
        {
            for (int i = 0; i < spawnAmount; i++)
            {
                var spawnPoint = Random.insideUnitCircle.normalized * spawnDistance;
                var worldSpawnPoint = new Vector3(spawnPoint.x, 0f, spawnPoint.y) + transform.position;
                if (!NavMesh.SamplePosition(worldSpawnPoint, out NavMeshHit hit, spawnDistance, NavMesh.AllAreas))
                    continue;
                var zombie = Instantiate(zombiePrefab, hit.position, Quaternion.identity);
                zombie.Setup(_player);
            }

            _timer = spawnTime;
        }
    }

    private void OnDestroy() => StopAllCoroutines();
}