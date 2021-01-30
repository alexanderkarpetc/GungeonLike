using System.Collections.Generic;
using GamePlay.Common;
using GamePlay.Weapons;
using UnityEngine;

namespace GamePlay.Player
{
  public class PlayerInitializer
  {
    public void Init(Weapon startingWeapon)
    {
      AppModel.Player().Backpack.AddWeapon(startingWeapon);
      AppModel.Player().Backpack.Ammo = new Dictionary<AmmoKind, int>
      {
        {AmmoKind.Pistol, WeaponStaticData.PistolAmmoCapacity},
        {AmmoKind.Riffle, WeaponStaticData.RiffleAmmoCapacity},
        {AmmoKind.Shell, WeaponStaticData.ShellAmmoCapacity},
        {AmmoKind.Energy, WeaponStaticData.EnergyAmmoCapacity},
        {AmmoKind.Bolt, WeaponStaticData.BoltAmmoCapacity},
      };
    }
  }
}