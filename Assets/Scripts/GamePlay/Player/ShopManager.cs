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
    private PickableItemView _pedestal;
    private ShopItemView _shopItem;

    public ShopManager()
    {
      _shopItem = Resources.Load("Prefabs/Player/ShopItem", typeof(ShopItemView)) as ShopItemView;
    }

    public ShopItemView SpawnRandomWeapon()
    {
      var shopItem = Object.Instantiate(_shopItem, (Vector2)AppModel.PlayerTransform().position + Vector2.up, Quaternion.identity);
      shopItem.GetComponent<NetworkObject>().Spawn();
      var weapon = AppModel.DropManager().GetAbsentWeapon();
      shopItem.SetDataServerRpc(weapon.Type);
      return shopItem;
    }
  }
}