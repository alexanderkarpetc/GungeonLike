using System;
using System.Collections.Generic;
using System.Linq;
using GamePlay.Player;
using GamePlay.Weapons;
using UnityEngine;

namespace GamePlay.Level
{
  public class PickableItemView : MonoBehaviour
  {
    [HideInInspector] public Weapon Weapon;
    [HideInInspector] public Dictionary<AmmoKind, int> Ammo;
    private int _index;

    private void OnTriggerEnter2D(Collider2D other)
    {
      if (other.CompareTag("Player"))
      {
        if (Weapon)
        {
          PickWeapon(Weapon);
        }
        if (!Ammo.IsNullOrEmpty())
        {
          AppModel.Player().Backpack.AddAmmo(Ammo);
        }
        Destroy(gameObject);
      }
    }

    private void PickWeapon(Weapon weapon)
    {
      AppModel.Player().Backpack.AddWeapon(weapon);
    }
  }
}
