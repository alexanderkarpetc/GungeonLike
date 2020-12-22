using System.Collections.Generic;
using GamePlay.Weapons;

namespace GamePlay.Common
{
  public static class WeaponStaticData
  {
    public static Dictionary<WeaponType, float> WeaponDamage = new Dictionary<WeaponType, float>
    {
      {WeaponType.Ak47, 1},
      {WeaponType.SpecialPistol, 1},
      {WeaponType.Magnum, 1},
      {WeaponType.ShotGun, 1},
    };
    public static List<WeaponType> AutomaticWeapons = new List<WeaponType> {WeaponType.Ak47};
    public static List<WeaponType> OneHandedWeapons = new List<WeaponType> {WeaponType.SpecialPistol};
    public static float TurretBulletSpeed = 7;
  }
}