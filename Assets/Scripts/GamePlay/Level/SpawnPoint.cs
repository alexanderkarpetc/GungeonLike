using System;
using GamePlay.Enemy;
using UnityEngine;

namespace GamePlay.Level
{
  public class SpawnPoint : MonoBehaviour
  {
    public EnemyController SpawnObject;
    public Action<EnemyController> OnSpawn;
    private void Start()
    {
      Invoke(nameof(Spawn), 0.5f);
    }

    public void Spawn()
    {
      var enemy = Instantiate(SpawnObject, transform.position, Quaternion.identity);
      OnSpawn(enemy);
      Destroy(gameObject);
    }
  }
}