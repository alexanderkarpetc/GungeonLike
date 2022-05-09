using System;
using GamePlay.Common;
using GamePlay.Enemy;
using UnityEngine;

namespace GamePlay.Weapons
{
  public class JetEngineImpact : MonoBehaviour
  {
    public Weapon Weapon;
    private void OnTriggerStay2D(Collider2D other)
    {
      if (other.CompareTag("Environment"))
      {
        other.GetComponent<Level.Environment>().DealDamage(AppModel.WeaponData().JetEngineDamage);
      }

      if (other.CompareTag("Enemy"))
      {
        var enemyController = other.GetComponent<EnemyController>();
        var direction = Weapon.DegreeToVector2(transform.rotation.eulerAngles.z);
        if (Weapon.IsInverted)
          direction *= -1;
        DamageManager.Hit(enemyController, Weapon.BaseDamage, direction.normalized * AppModel.WeaponData().JetEngineImpulse);
      }
    }
  }
}