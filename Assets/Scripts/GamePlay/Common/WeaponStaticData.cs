using System.Collections.Generic;
using GamePlay.Player;
using GamePlay.Weapons;

namespace GamePlay.Common
{
  public class WeaponStaticData
  {
    public Dictionary<WeaponType, WeaponInfo> WeaponInfos = new Dictionary<WeaponType, WeaponInfo>();
    public Dictionary<AmmoKind, int> AmmoCapacity = new Dictionary<AmmoKind, int>
    {
      {AmmoKind.Pistol, 50},
      {AmmoKind.Riffle, 100},
      {AmmoKind.Shell, 20},
      {AmmoKind.Bolt, 30},
      {AmmoKind.Energy, 500},
    };
    public Dictionary<AmmoKind, int> AmmoFillAmount = new Dictionary<AmmoKind, int>
    {
      {AmmoKind.Pistol, 10},
      {AmmoKind.Riffle, 20},
      {AmmoKind.Shell, 6},
      {AmmoKind.Bolt, 5},
      {AmmoKind.Energy, 50},
    };
    public List<WeaponType> AutomaticWeapons = new List<WeaponType> {WeaponType.Ak47};
    public List<WeaponType> SemiAutoWeapons = new List<WeaponType> {WeaponType.SpecialPistol, WeaponType.EnemyMagnum, WeaponType.EnemyShotGun, WeaponType.Crossbow};
    public List<WeaponType> ChargeWeapons = new List<WeaponType> {WeaponType.JetEngine};
    public float TurretBulletSpeed = 7;
    public float JetEngineDamage = 0.1f;
    public float JetEngineImpulse = 0.1f;

    public int GetAmmoAmountForKind(AmmoKind ammoKind)
    {
      return AmmoFillAmount[ammoKind];
    }
    
    public WeaponInfo GetWeaponInfo(WeaponType type)
    {
      return WeaponInfos[type];
    }
  }
}