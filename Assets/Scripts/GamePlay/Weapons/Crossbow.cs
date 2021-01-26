using GamePlay.Common;
using UnityEngine;

namespace GamePlay.Weapons
{
  public class Crossbow : Weapon
  {
    protected override void SpawnProjectiles()
    {
      var go = Instantiate(_projectile, _shootPoint.position, Quaternion.identity);
      go.transform.SetParent(AppModel.BulletContainer().transform);
      var projectile = go.GetComponent<Projectile>();
      projectile.IsPlayerBullet = IsPlayers;
      projectile.Weapon = this;
      projectile.Speed = _bulletSpeed;
      projectile.Impulse = _impulse;

      projectile.Direction = Vector2.right;
      projectile.transform.rotation = Quaternion.Euler(0,0, transform.rotation.eulerAngles.z);
      if (IsInverted)
        projectile.Direction *= -1;
    }
  }
}