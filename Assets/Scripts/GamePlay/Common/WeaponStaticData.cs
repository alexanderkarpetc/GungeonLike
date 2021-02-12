using System.Collections.Generic;
using GamePlay.Player;
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
    public static Dictionary<AmmoKind, int> AmmoCapacity = new Dictionary<AmmoKind, int>
    {
      {AmmoKind.Pistol, 50},
      {AmmoKind.Riffle, 100},
      {AmmoKind.Shell, 20},
      {AmmoKind.Bolt, 30},
      {AmmoKind.Energy, 500},
    };
    public static Dictionary<AmmoKind, int> AmmoFillAmount = new Dictionary<AmmoKind, int>
    {
      {AmmoKind.Pistol, 10},
      {AmmoKind.Riffle, 20},
      {AmmoKind.Shell, 6},
      {AmmoKind.Bolt, 5},
      {AmmoKind.Energy, 50},
    };
    public static List<WeaponType> AutomaticWeapons = new List<WeaponType> {WeaponType.Ak47};
    public static List<WeaponType> SemiAutoWeapons = new List<WeaponType> {WeaponType.SpecialPistol, WeaponType.Magnum, WeaponType.ShotGun, WeaponType.Crossbow};
    public static List<WeaponType> ChargeWeapons = new List<WeaponType> {WeaponType.JetEngine};
    public static float TurretBulletSpeed = 7;
    public static float JetEngineDamage = 0.1f;
    public static float JetEngineImpulse = 0.1f;

    public static int GetAmmoAmountForKind(AmmoKind ammoKind)
    {
      return AmmoFillAmount[ammoKind];
    }
  }
}