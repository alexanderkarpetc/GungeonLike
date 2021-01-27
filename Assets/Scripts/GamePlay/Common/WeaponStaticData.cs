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
      {WeaponType.Crossbow, 10},
    };
    public static List<WeaponType> AutomaticWeapons = new List<WeaponType> {WeaponType.Ak47};
    public static List<WeaponType> SemiAutoWeapons = new List<WeaponType> {WeaponType.SpecialPistol, WeaponType.Magnum, WeaponType.ShotGun, WeaponType.Crossbow};
    public static List<WeaponType> ChargeWeapons = new List<WeaponType> {WeaponType.JetEngine};
    public static float TurretBulletSpeed = 7;
    public static float JetEngineDamage = 0.1f;
    public static float JetEngineImpulse = 0.1f;
    public static int PistolAmmoCapacity = 50;
    public static int RiffleAmmoCapacity = 100;
    public static int ShellAmmoCapacity = 20;
    public static int EnergyAmmoCapacity = 500;
    public static int BoltAmmoCapacity = 30;
  }
}