using System;
using System.Collections.Generic;
using System.Linq;
using GamePlay.Enemy;
using UnityEngine;

namespace GamePlay.Level.Controllers
{
  public class StraightRoomController : MonoBehaviour
  {
    private List<EnemyController> _enemies = new List<EnemyController>();
    private RoomData _roomData;

    private void Start()
    {
      _roomData = GetComponent<RoomData>();
      AppModel.StraightRoomController.ReInitAstar();
      AppModel.CurrentRoom = this;
      switch (_roomData.kind)
      {
        case RoomKind.Normal:
          SpawnEnemies();
          break;
        case RoomKind.Boss:
          SpawnBoss();
          break;
        case RoomKind.Treasure:
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    private void SpawnEnemies()
    {
      _roomData.points.ForEach(x =>
      {
        var randomEnemy = AppModel.EnemyFactory().GetRandomEnemy();
        SpawnEnemy(randomEnemy, x.position);
      });
    }
    private void SpawnBoss()
    {
      var index = AppModel.random.NextInt(_roomData.enemies.Count);
      var spawnPoint = Instantiate(AppModel.SpawnPointPrefab(), _roomData.points.First().position, Quaternion.identity);
      spawnPoint.OnSpawn += OnSpawn;
      spawnPoint.SpawnObject = _roomData.enemies[index];
    }

    private void SpawnEnemy(EnemySetup enemySetup, Vector3 position)
    {
      var spawnPoint = Instantiate(AppModel.SpawnPointPrefab(), position, Quaternion.identity);
      spawnPoint.OnSpawn += OnSpawn;
      spawnPoint.SpawnObject = enemySetup.EnemyObject;
    }
    private void OnSpawn(EnemyController enemyController)
    {
      enemyController.OnDeath += OnEnemyDeath;
      _enemies.Add(enemyController);
    }

    private void OnEnemyDeath(EnemyController enemyController)
    {
      _enemies.Remove(enemyController);
      if (_enemies.Count == 0)
        OpenDoors();
    }

    private void OpenDoors()
    {
      _roomData.exits.ForEach(x => x.IsActive = true);
    }

    public RoomData GetRoomData()
    {
      return _roomData;
    }
  }
}