using System;
using GamePlay.Weapons;

namespace GamePlay.Player
{
  public class ShopItemInteractable : Interactable
  {
    public int _price;
    public Weapon _weapon;
    public override void Interact()
    {
      if (AppModel.Player().Backpack.GetCoins() < _price)
      {
        return;
      }
      
      AppModel.Player().Backpack.WithdrawResource(ResourceKind.Coins, _price);
      AppModel.Player().Backpack.AddWeapon(_weapon);
    }
  }
}