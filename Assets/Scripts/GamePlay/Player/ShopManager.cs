using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using GamePlay.Common;
using GamePlay.Enemy;
using GamePlay.Extensions;
using GamePlay.Level;
using GamePlay.Weapons;
using Unity.Netcode;
using UnityEngine;

namespace GamePlay.Player
{
  public class ShopManager
  {
    private AutoPickableItemView _pedestal;
    private PickableItemView _pickableItem;

    public ShopManager()
    {
      _pickableItem = Resources.Load("Prefabs/Player/ShopItem", typeof(PickableItemView)) as PickableItemView;
    }

    public PickableItemView SpawnRandomWeapon()
    {
      var shopItem = Object.Instantiate(_pickableItem, (Vector2)AppModel.PlayerTransform().position + Vector2.up, Quaternion.identity);
      shopItem.GetComponent<NetworkObject>().Spawn();
      var weapon = AppModel.DropManager().GetAbsentWeapon();
      shopItem.WeaponType.Value = weapon.Type;
      shopItem.HasWeapon.Value = true;
      return shopItem;
    }
  }
}