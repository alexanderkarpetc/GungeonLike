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
        {AmmoKind.Pistol, WeaponStaticData.AmmoCapacity[AmmoKind.Pistol] / 3},
        {AmmoKind.Riffle, WeaponStaticData.AmmoCapacity[AmmoKind.Riffle] / 3},
        {AmmoKind.Shell, WeaponStaticData.AmmoCapacity[AmmoKind.Shell] / 3},
        {AmmoKind.Energy, WeaponStaticData.AmmoCapacity[AmmoKind.Energy] / 3},
        {AmmoKind.Bolt, WeaponStaticData.AmmoCapacity[AmmoKind.Bolt] / 3},
      };
    }
  }
}