using GamePlay.Enemy;
using GamePlay.Level;
using GamePlay.Player;
using GamePlay.Weapons;
using UnityEngine;

namespace GamePlay.Common
{
  public static class DamageManager
  {
    public static void Explode(Vector3 explodePoint, float radius, float damage)
    {
      var hits = Physics2D.OverlapCircleAll(explodePoint, radius);
      foreach (var hit in hits)
      {
        if (hit.CompareTag("Enemy"))
        {
          Hit(hit.GetComponent<EnemyController>(), damage);
        }
        if (hit.CompareTag("Player"))
        {
          HitPlayer(hit.GetComponent<PlayerController>());
        }
        if (hit.CompareTag("Environment"))
        {
          var environment = hit.GetComponent<Environment>();
          environment.DealDamageServerRpc(damage);
        }
      }
    }
    public static void Hit(EnemyController enemyController, float damage, Vector2? impulse)
    {
      enemyController.DealDamageServerRpc(damage);
      if(impulse != null)
        enemyController.GiveImpulse(impulse.Value);
    }

    public static void Hit(Environment environment, float damage)
    {
      environment.DealDamageServerRpc(damage);
    }

    public static void Hit(EnemyController enemyController, float damage)
    {
      enemyController.DealDamageServerRpc(damage);
    }

    public static void HitPlayer(PlayerController playerController)
    {
      playerController.Hit();
    }
  }
}