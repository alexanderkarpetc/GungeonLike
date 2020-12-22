using System.Collections.Generic;
using GamePlay.Weapons;
using UnityEngine;

namespace GamePlay.Player
{
  public class Backpack
  {
    private readonly List<Weapon> _weapons = new List<Weapon>();
    private int _currentWeaponIndex = -1;
    public int gold;
    public int keys;

    public void AddWeapon(Weapon newWeapon)
    {
      _weapons.Add(newWeapon);
      _currentWeaponIndex = _weapons.Count - 1;
      newWeapon.State = new WeaponState{bulletsLeft = newWeapon.MagazineSize};
      newWeapon.IsPlayers = true;
      SelectWeapon(newWeapon);
    }

    public void SelectWeapon(Weapon newWeapon)
    {
      var weaponSlot = AppModel.PlayerTransform().Find("WeaponSlot");
      if(weaponSlot.childCount != 0)
        GameObject.Destroy(weaponSlot.transform.GetChild(0).gameObject);
      var weapon = GameObject.Instantiate(newWeapon, weaponSlot);
      weapon.State = newWeapon.State;
      AppModel.Player().SetWeapon(weapon);
      var playerWeaponTurn = weaponSlot.GetComponent<PlayerWeaponTurn>();
      playerWeaponTurn.Weapon = weapon;
      playerWeaponTurn.ReInit();
    }

    public void NextWeapon()
    {
      if(_weapons.Count == 1)
        return;
      _currentWeaponIndex = _currentWeaponIndex == _weapons.Count - 1 ? 0 : _currentWeaponIndex + 1;
      SelectWeapon(_weapons[_currentWeaponIndex]);
    }
    
    public void PreviousWeapon()
    {
      if(_weapons.Count == 1)
        return;
      _currentWeaponIndex = _currentWeaponIndex == 0 ?_weapons.Count - 1 : _currentWeaponIndex - 1;
      SelectWeapon(_weapons[_currentWeaponIndex]);
    }
  }
}