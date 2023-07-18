using GamePlay.Weapons;
using UnityEngine;
using UnityEngine.EventSystems;

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
      if (EventSystem.current.IsPointerOverGameObject())
      {
        // The mouse is over a UI element, so do not process other mouse input.
        return;
      }

      if (Weapon == null)
      {
        Weapon = _weaponSlot.GetChild(0).GetComponent<Weapon>();
        Weapon.IsPlayers = true;
      }

      if (Weapon.ShootingType == WeaponShootingType.SemiAuto && Input.GetMouseButtonDown(0))
      {
        Weapon.TryShoot();
        return;
      }      
      if (Weapon.ShootingType == WeaponShootingType.Automatic && Input.GetMouseButton(0))
      {
        Weapon.TryShoot();
        return;
      }
      if (Weapon.ShootingType == WeaponShootingType.Charged && Input.GetMouseButton(0))
      {
        ((JetEngineWeapon)Weapon).StartCharge();
        return;
      }
      if (Weapon.ShootingType == WeaponShootingType.Charged && !Input.GetMouseButton(0))
      {
        ((JetEngineWeapon)Weapon).StopCharge();
        return;
      }
    }
  }
}