using System;
using GamePlay.Level;
using GamePlay.Weapons;
using UnityEngine;

namespace GamePlay.Player
{
  public class ShopItemInteractable : Interactable
  {
    public int Price;
    public Weapon Weapon;
    public GameObject View;

    public override void Interact(GameObject interactObj)
    {
      if (AppModel.Player().Backpack.GetCoins() < Price)
      {
        return;
      }
      
      AppModel.Player().Backpack.WithdrawResource(ResourceKind.Coins, Price);
      AppModel.Player().Backpack.AddWeapon(Weapon);
      GameObject.Destroy(View);
    }
  }
}