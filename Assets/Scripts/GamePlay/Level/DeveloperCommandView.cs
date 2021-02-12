using UnityEngine;

namespace GamePlay.Level
{
  public class DeveloperCommandView : MonoBehaviour
  {
    public void SpawnRandomEnemy()
    {
      var randomEnemy = AppModel.EnemyFactory().GetRandomEnemy();
      var spawnPoint = Instantiate(AppModel.SpawnPointPrefab(), Vector3.zero, Quaternion.identity);
      spawnPoint.SpawnObject = randomEnemy.EnemyObject;
    }
  }
}