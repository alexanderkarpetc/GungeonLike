using System;
using GamePlay.Common;
using GamePlay.Weapons;
using UnityEngine;

namespace GamePlay.Player
{
  public class PlayerShooting : MonoBehaviour
  {
    [HideInInspector] public Weapon Weapon;
    [SerializeField] public Transform _weaponSlot;

    void Update()
    {
      CheckShoot();
    }

    private void CheckShoot()
    {
      if (Weapon == null)
      {
        Weapon = _weaponSlot.GetChild(0).GetComponent<Weapon>();
        Weapon.IsPlayers = true;
      }

      if (AppModel.WeaponData().AutomaticWeapons.Contains(Weapon.Type) && Input.GetMouseButton(0))
      {
        Weapon.TryShoot();
        return;
      }      
      if (AppModel.WeaponData().SemiAutoWeapons.Contains(Weapon.Type) && Input.GetMouseButtonDown(0))
      {
        Weapon.TryShoot();
        return;
      }
      if (AppModel.WeaponData().ChargeWeapons.Contains(Weapon.Type) && Input.GetMouseButton(0))
      {
        ((JetEngineWeapon)Weapon).StartCharge();
        return;
      }
      if (AppModel.WeaponData().ChargeWeapons.Contains(Weapon.Type) && !Input.GetMouseButton(0))
      {
        ((JetEngineWeapon)Weapon).StopCharge();
        return;
      }
    }
  }
}