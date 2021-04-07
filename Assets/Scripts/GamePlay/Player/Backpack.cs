using System;
using System.Collections.Generic;
using GamePlay.Common;
using GamePlay.Weapons;
using UnityEngine;

namespace GamePlay.Player
{
  public class Backpack
  {
    private readonly List<Weapon> _weapons = new List<Weapon>();
    private int _currentWeaponIndex = -1;
    public Dictionary<AmmoKind, int> Ammo;

    private Dictionary<ResourceKind, int> _resources;
    public event Action<Dictionary<AmmoKind, int>> OnAmmoChange;
    public event Action<Tuple<ResourceKind, int>> OnResourcesChange;

    public Dictionary<ResourceKind, int> Resources
    {
      get => _resources;
      set
      {
        _resources = value;
        foreach (var valuePair in _resources)
        {
          OnResourcesChange.NullSafeInvoke(new Tuple<ResourceKind, int>(valuePair.Key, valuePair.Value));
        }
      }
    }

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

    public void AddAmmo(Dictionary<AmmoKind,int> ammo)
    {
      foreach (var pair in ammo)
      {
        Ammo[pair.Key] = Mathf.Clamp(pair.Value+Ammo[pair.Key], 0, WeaponStaticData.AmmoCapacity[pair.Key]);
      }

      OnAmmoChange.NullSafeInvoke(ammo);
    }
    
    public void AddResource(Tuple<ResourceKind, int> resource)
    {
      Resources[resource.Item1] = resource.Item2 + Resources.GetValueOrDefault(resource.Item1);
      OnResourcesChange.NullSafeInvoke(resource);
    }
    
    public void WithdrawResource(Tuple<ResourceKind, int> resource)
    {
      Resources[resource.Item1] = Resources.GetValueOrDefault(resource.Item1) - resource.Item2;
      OnResourcesChange.NullSafeInvoke(resource);
    }
  }
}