using System.Collections;
using System.Collections.Generic;
using DarkHavoc.CustomUtils;
using UnityEngine;

namespace LevelGeneration
{
    public class LevelManager : Singleton<LevelManager>
    {
        [SerializeField] private PlayerController playerPrefab;
        [SerializeField] private GrannyController grannyPrefab;
        
        //[SerializeField] private ExitDoor exitDoorPrefab;
        // [SerializeField] private BiomeBestiary bestiary;

        private LevelGenerator _levelGenerator;

        private Vector3 _spawnPoint;

        private void Start()
        {
            _levelGenerator = LevelGenerator.Instance;
            StartCoroutine(StartLevelAsync());
        }

        private IEnumerator StartLevelAsync()
        {
            yield return null;
            _levelGenerator.GenerateLevel();
            yield return null;
            SpawnEnemies();
            SpawnInstantiables();
            yield return null;
            CreatePlayer();
            CreateExit();
        }

        private void CreateExit()
        {
            GridRoomData exitRoomData = _levelGenerator.ExitRoom;
            Vector3 position = _levelGenerator.GetWorldPosition(exitRoomData);
            // Instantiate(exitDoorPrefab, position, Quaternion.identity);
        }

        private void SpawnEnemies()
        {
            List<Vector3> spawnPoints = _levelGenerator.WorldPositionSpawnPoints;

            foreach (var spawnPoint in spawnPoints)
            {
                // TODO: Check if spawn zombies using this.
                // int index = Random.Range(0, bestiary.Bestiary.Length);
                // Instantiate(bestiary.Bestiary[index], spawnPoint, Quaternion.identity);
            }
        }

        private void SpawnInstantiables()
        {
            List<Instantiable> instantiables = _levelGenerator.Instantiables;

            foreach (var instantiable in instantiables)
                Instantiate(instantiable.Prefab, instantiable.WorldPosition, Quaternion.identity);
        }

        private void CreatePlayer()
        {
            var playerSpawnPoint = _levelGenerator.GetWorldPosition(_levelGenerator.InitialRoom);
            var player = Instantiate(playerPrefab, playerSpawnPoint, Quaternion.identity);
            var granny = Instantiate(grannyPrefab, playerSpawnPoint + Vector3.forward, Quaternion.identity);
        }

        public void ExitLevel()
        {
            // TODO: Win game.
            // _gameManager.GoToNextLevel();
        }
    }
}