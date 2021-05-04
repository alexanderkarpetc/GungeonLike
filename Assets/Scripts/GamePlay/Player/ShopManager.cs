using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using GamePlay.Common;
using GamePlay.Enemy;
using GamePlay.Extensions;
using GamePlay.Level;
using GamePlay.Weapons;
using UnityEngine;

namespace GamePlay.Player
{
  public class ShopManager
  {
    private PickableItemView _pedestal;
    private GameObject _parentObj;
    private ShopItemView _shopItem;

    public ShopManager()
    {
      if (_parentObj == null)
        _parentObj = Util.InitParentIfNeed("Items");
      _shopItem = Resources.Load("Prefabs/Player/ShopItem", typeof(ShopItemView)) as ShopItemView;
    }

    public ShopItemView SpawnRandomWeapon()
    {
      var shopItem = Object.Instantiate(_shopItem, Vector3.zero, Quaternion.identity, _parentObj.transform);
      var weapon = AppModel.DropManager().GetAbsentWeapon();
      shopItem.SetData(weapon);
      return shopItem;
    }
  }
}