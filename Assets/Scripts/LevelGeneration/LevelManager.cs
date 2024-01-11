using System;
using System.Collections;
using System.Collections.Generic;
using DarkHavoc.CustomUtils;
using UnityEngine;

namespace LevelGeneration
{
    public class LevelManager : Singleton<LevelManager>
    {
        public float NormalizedTimer => Timer / maxTime;
        public float Timer { get; private set; }

        [SerializeField] private PlayerController playerPrefab;
        [SerializeField] private GrannyController grannyPrefab;

        [SerializeField] private float maxTime = 60;

        private LevelGenerator _levelGenerator;
        private Vector3 _spawnPoint;

        private void Start()
        {
            _levelGenerator = LevelGenerator.Instance;
            StartCoroutine(StartLevelAsync());
            Timer = maxTime;
        }

        private void Update()
        {
            if (Timer > 0f) Timer -= Time.deltaTime;
        }

        private IEnumerator StartLevelAsync()
        {
            yield return null;
            _levelGenerator.GenerateLevel();
            yield return null;
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

        // TODO: Check if necessary.
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