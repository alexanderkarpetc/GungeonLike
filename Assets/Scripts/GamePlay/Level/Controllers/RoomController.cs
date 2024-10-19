using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GamePlay.Enemy;
using Unity.Netcode;
using UnityEngine;

namespace GamePlay.Level.Controllers
{
    public class RoomController : NetworkBehaviour
    {
        private Transform _env;
        private List<EnemyController> _enemies = new List<EnemyController>();
        private List<Transform> _envs = new List<Transform>();

        private RoomData _currentRoom;

        // Called from server
        public void Init(RoomData currentRoom)
        {
            _currentRoom = currentRoom;
            _env = _currentRoom.transform.Find("Env").transform;
            // _env.gameObject.SetActive(true);
            
            SpawnEnv().Forget();
            foreach (var door in _currentRoom.exits)
            {
                door.GetComponent<NetworkObject>().Spawn();
                _envs.Add(door.transform);
            }
            AppModel.StraightRoomController.ReInitAstar();
            AppModel.CurrentRoom = this;

            switch (_currentRoom.kind)
            {
                case RoomKind.Normal:
                    SpawnEnemiesServerRpc();
                    break;
                case RoomKind.Boss:
                    SpawnBossServerRpc();
                    break;
                case RoomKind.Treasure:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private async UniTask SpawnEnv()
        {
            for (var i = 0; i < _env.childCount; i++)
            {
                await UniTask.Yield();
                var envObj = _env.GetChild(i);
                Debug.Log($"envObj: {envObj.name}");
                envObj.GetComponent<NetworkObject>().Spawn();
                _envs.Add(envObj);
            }
        }

        [ServerRpc]
        private void SpawnEnemiesServerRpc()
        {
            foreach (var point in _currentRoom.points)
            {
                var randomEnemy = AppModel.EnemyFactory().GetRandomEnemy();
                SpawnEnemy(randomEnemy, point.position);
            }
        }

        [ServerRpc]
        private void SpawnBossServerRpc()
        {
            var index = AppModel.random.NextInt(_currentRoom.enemies.Count);
            var spawnPoint = Instantiate(AppModel.SpawnPointPrefab(), _currentRoom.points.First().position, Quaternion.identity);
            var spawnPointNetworkObject = spawnPoint.GetComponent<NetworkObject>();

            if (spawnPointNetworkObject != null)
            {
                spawnPointNetworkObject.Spawn(); // Ensure the spawn point is networked
            }

            spawnPoint.OnSpawn += OnSpawn;
            spawnPoint.SpawnObject = _currentRoom.enemies[index];
        }

        private void SpawnEnemy(EnemySetup enemySetup, Vector3 position)
        {
            var spawnPoint = Instantiate(AppModel.SpawnPointPrefab(), position, Quaternion.identity);
            var spawnPointNetworkObject = spawnPoint.GetComponent<NetworkObject>();

            if (spawnPointNetworkObject != null)
            {
                spawnPointNetworkObject.Spawn(); // Spawn the enemy across the network
            }

            spawnPoint.OnSpawn += OnSpawn;
            spawnPoint.SpawnObject = enemySetup.EnemyObject;
        }

        private void OnSpawn(EnemyController enemyController)
        {
            if (IsServer)
            {
                enemyController.OnDeath += OnEnemyDeath;
                _enemies.Add(enemyController);
            }
        }

        private void OnEnemyDeath(EnemyController enemyController)
        {
            if (IsServer)
            {
                _enemies.Remove(enemyController);
                if (_enemies.Count == 0)
                    OpenDoors();
            }
        }

        private void OpenDoors()
        {
            foreach (var exit in _currentRoom.exits)
            {
                exit.IsActive = true;
            }

            // If the door activation should be synchronized, you can use a ClientRpc
            OpenDoorsClientRpc();
        }

        [ClientRpc]
        private void OpenDoorsClientRpc()
        {
            foreach (var exit in _currentRoom.exits)
            {
                // Update the door state on all clients
                exit.IsActive = true;
            }
        }

        public RoomData GetRoomData()
        {
            return _currentRoom;
        }

        public void DespawnEnv()
        {
            foreach (var envObj in _envs)
            {
                var networkObject = envObj.GetComponent<NetworkObject>();
                networkObject.Despawn();
            }
        }
    }
}