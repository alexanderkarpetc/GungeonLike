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
    public ResourcePack ResourceValue;

    private int _index;

    [Serializable]
    public class ResourcePack
    {
      public ResourceKind resourceKind;
      public int amount;
    }

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
        if (ResourceValue != null)
        {
          AppModel.Player().Backpack.AddResource(new Tuple<ResourceKind, int>(ResourceValue.resourceKind, ResourceValue.amount));
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
