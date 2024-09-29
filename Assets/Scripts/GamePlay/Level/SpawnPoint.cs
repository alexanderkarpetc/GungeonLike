using System;
using GamePlay.Enemy;
using Unity.Netcode;
using UnityEngine;

namespace GamePlay.Level
{
  public class SpawnPoint : NetworkBehaviour
  {
    public EnemyController SpawnObject;
    public Action<EnemyController> OnSpawn;
    private void Start()
    {
      if (IsServer && !IsSpawned)
      {
        GetComponent<NetworkObject>().Spawn();
      }
      Invoke(nameof(SpawnEnemy), 0.5f);
    }

    public void SpawnEnemy()
    {
      var enemy = Instantiate(SpawnObject, transform.position, Quaternion.identity);
      OnSpawn.NullSafeInvoke(enemy);
      Destroy(gameObject);
    }
  }
}