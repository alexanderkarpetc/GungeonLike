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
        {AmmoKind.Pistol, AppModel.WeaponData().AmmoCapacity[AmmoKind.Pistol] / 3},
        {AmmoKind.Riffle, AppModel.WeaponData().AmmoCapacity[AmmoKind.Riffle] / 3},
        {AmmoKind.Shell, AppModel.WeaponData().AmmoCapacity[AmmoKind.Shell] / 3},
        {AmmoKind.Energy, AppModel.WeaponData().AmmoCapacity[AmmoKind.Energy] / 3},
        {AmmoKind.Bolt, AppModel.WeaponData().AmmoCapacity[AmmoKind.Bolt] / 3},
        {AmmoKind.Bomb, AppModel.WeaponData().AmmoCapacity[AmmoKind.Bomb] / 5},
      };
      AppModel.Player().Backpack.Resources = new Dictionary<ResourceKind, int>
      {
        {ResourceKind.Coins, 10},
      };
    }
  }
}