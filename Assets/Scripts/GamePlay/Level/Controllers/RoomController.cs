using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GamePlay.Enemy;
using GamePlay.Player;
using Unity.Netcode;
using UnityEngine;

namespace GamePlay.Level.Controllers
{
    public class RoomController : NetworkBehaviour
    {
        [HideInInspector] public List<NextRoomInteractable> Doors = new List<NextRoomInteractable>();
        [SerializeField] private NetworkPrefabsList _prefabsList;
        [SerializeField] private List<Transform> _envs;
        [SerializeField] private List<GameObject> _envPrefabCache;

        private Transform _env;
        private List<EnemyController> _enemies = new List<EnemyController>();

        private RoomData _currentRoom;

        private void Start()
        {
            AppModel.RoomController = this;
        }

        // Called from server
        public void Init()
        {
            foreach (var pref in _prefabsList.PrefabList)
            {
                _envPrefabCache.Add(pref.Prefab);  
            }
        }

        // Called from server
        public void Set(RoomData currentRoom, bool isStartingRoom)
        {
            _currentRoom = currentRoom;
            _env = _currentRoom.transform.Find("Env");
            // _env.gameObject.SetActive(true);
            
            SpawnEnv();
            SpawnDoors(isStartingRoom);
            AppModel.LevelController.ReInitAstar();

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

        // todo: rework it somehow
        private void SpawnEnv()
        {
            var envs = _env.GetComponentsInChildren<Environment>();
            envs.ToList().ForEach(env =>
            {
                var prefabToInit = _envPrefabCache.Find(prefab => prefab.GetComponent<Environment>()?.Type == env.Type);
                var envObj = Instantiate(prefabToInit, env.transform.position, Quaternion.identity);
                var networkObject = envObj.GetComponent<NetworkObject>();
                networkObject.Spawn();
                _envs.Add(envObj.transform);
            });
        }

        // todo: rework it somehow
        private void SpawnDoors(bool isStartingRoom)
        {
            foreach (var door in _currentRoom.exits)
            {
                var prefabToInit = _envPrefabCache.Find(prefab => prefab.GetComponent<NextRoomInteractable>()?.DoorType == door.DoorType);
                var envObj = Instantiate(prefabToInit, door.transform.position, Quaternion.identity);
                if(isStartingRoom)
                    envObj.GetComponent<NextRoomInteractable>().IsActive = true;
                envObj.GetComponent<NetworkObject>().Spawn();
                _envs.Add(envObj.transform);
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
                    OpenDoorsServerRpc();
            }
        }

        [ServerRpc]
        private void OpenDoorsServerRpc()
        {
            OpenDoorsClientRpc();
        }

        [ClientRpc]
        private void OpenDoorsClientRpc()
        {
            Doors.ForEach(door => door.IsActive = true);
        }

        public RoomData GetRoomData()
        {
            return _currentRoom;
        }

        public void DespawnEnv()
        {
            foreach (var envObj in _envs)
            {
                if (envObj == null)
                    continue;
                var networkObject = envObj.GetComponent<NetworkObject>();
                networkObject.Despawn();
            }

            _envs.Clear();
        }
    }
}