using GamePlay.Weapons;
using UnityEngine;

namespace GamePlay.Player
{
  public class PlayerInitializer
  {
    public void Init(Weapon startingWeapon)
    {
      ChangeWeapon(startingWeapon);
    }

    private static void ChangeWeapon(Weapon newWeapon)
    {
      AppModel.Player().SetWeapon(newWeapon);
      var weaponSlot = AppModel.PlayerTransform().Find("WeaponSlot");
      var weapon = GameObject.Instantiate(newWeapon, weaponSlot);
      weaponSlot.GetComponent<PlayerWeaponTurn>().Weapon = weapon;
    }
  }
}