using System.Collections.Generic;
using GamePlay.Weapons;
using UnityEngine;

namespace GamePlay.Player
{
  public class Backpack
  {
    private readonly List<Weapon> _weapons = new List<Weapon>();
    public int gold;
    public int keys;
    
    public void AddWeapon(Weapon newWeapon)
    {
      _weapons.Add(newWeapon);
      SelectWeapon(newWeapon);
    }

    public void SelectWeapon(Weapon newWeapon)
    {
      var weaponSlot = AppModel.PlayerTransform().Find("WeaponSlot");
      var weapon = GameObject.Instantiate(newWeapon, weaponSlot);
      AppModel.Player().SetWeapon(weapon);
      weaponSlot.GetComponent<PlayerWeaponTurn>().Weapon = weapon;
    }
  }
}