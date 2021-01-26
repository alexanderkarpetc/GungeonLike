using GamePlay.Enemy;
using GamePlay.Level;
using GamePlay.Player;
using GamePlay.Weapons;
using UnityEngine;

namespace GamePlay.Common
{
  public static class DamageManager
  {
    public static void Hit(EnemyController enemyController, Weapon weapon, Vector2? impulse)
    {
      enemyController.DealDamage(weapon.BaseDamage);
      if(impulse != null)
        enemyController.OnHit(impulse.Value);
    }

    public static void Hit(Environment environment, Weapon weapon)
    {
      environment.DealDamage(weapon.BaseDamage);
    }

    public static void Hit(EnemyController enemyController, Barrel barrel)
    {
      enemyController.DealDamage(barrel.Damage);
    }

    public static void HitPlayer(PlayerController playerController)
    {
      playerController.Hit();
    }
  }
}