using System.Collections.Generic;
using GamePlay.Weapons;

namespace GamePlay.Common
{
  public static class WeaponStaticData
  {
    public static Dictionary<WeaponType, float> WeaponDamage = new Dictionary<WeaponType, float>
    {
      {WeaponType.Ak47, 1},
      {WeaponType.Magnum, 1},
      {WeaponType.ShotGun, 1},
    };
  }
}