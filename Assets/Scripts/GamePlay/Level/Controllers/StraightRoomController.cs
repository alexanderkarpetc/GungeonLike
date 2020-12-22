using System.Collections.Generic;
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
      SpawnEnemies();
    }

    private void SpawnEnemies()
    {
      _roomData.points.ForEach(x =>
      {
        var randomEnemy = AppModel.EnemyFactory().GetRandomEnemy();
        SpawnEnemy(randomEnemy, x.position);
      });
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
      _roomData.exits.ForEach(x => x.CanGoNextRoom = true);
    }
  }
}