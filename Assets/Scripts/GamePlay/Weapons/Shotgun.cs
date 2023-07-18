using GamePlay.Common;
using UnityEngine;

namespace GamePlay.Weapons
{
  public class Shotgun : Weapon
  {
    public int BulletsCount;
    public int DispersionAngle;
    protected override void InitProjectiles()
    {
      var angleShift = DispersionAngle / BulletsCount;
      var currentRotation = transform.rotation.eulerAngles.z + DispersionAngle / 2;
      
      for (var i = 0; i < BulletsCount; i++)
      {
        var go = BulletPoolManager.Instance.GetBulletFromPool(_projectile, _shootPoint.position, Quaternion.identity, projectileName);
        go.transform.SetParent(AppModel.BulletContainer().transform);
        var projectile = go.GetComponent<Projectile>();
        projectile.IsPlayerBullet = IsPlayers;
        projectile.Damage = BaseDamage;
        projectile.Speed = _bulletSpeed;
        projectile.Direction = DegreeToVector2(currentRotation);
        if (IsInverted)
          projectile.Direction *= -1;
        currentRotation -= angleShift;
      }
    }
  }
}