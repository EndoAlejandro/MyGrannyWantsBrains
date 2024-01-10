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
    private void Awake() => _player = GetComponent<PlayerController>();
    private void Start() => StartCoroutine(SpawnZombiesAsync());

    private IEnumerator SpawnZombiesAsync()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnTime);
            for (int i = 0; i < spawnAmount; i++)
            {
                var spawnPoint = Random.insideUnitCircle.normalized * spawnDistance;
                var worldSpawnPoint = new Vector3(spawnPoint.x, 0f, spawnPoint.y);
                if (!NavMesh.SamplePosition(worldSpawnPoint, out NavMeshHit hit, spawnDistance, NavMesh.AllAreas))
                    continue;
                var zombie = Instantiate(zombiePrefab, hit.position, Quaternion.identity);
                zombie.Setup(_player);
            }
        }
    }

    private void OnDestroy() => StopAllCoroutines();
}