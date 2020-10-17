using UnityEngine;

namespace Weapons
{
  public class Shotgun : Weapon
  {
    public int BulletsCount;
    public int DispersionAngle;
    protected override void SpawnProjectiles()
    {
      var angleShift = DispersionAngle / BulletsCount;
      var currentRotation = transform.rotation.eulerAngles.z + DispersionAngle / 2;
      
      for (var i = 0; i < BulletsCount; i++)
      {
        var go = Instantiate(_projectile, _shootPoint.position, Quaternion.Euler(0,0,currentRotation));
        currentRotation -= angleShift;
        var projectile = go.GetComponent<Projectile>();
        projectile.IsInverted = IsInverted;
        projectile.IsPlayerBullet = IsPlayers;
        projectile.Damage = _damage;
        projectile.Speed = _bulletSpeed;
      }
    }
  }
}