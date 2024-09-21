using GamePlay.Player;
using GamePlay.Weapons;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay.Level
{
  public class ShopItemView : Interactable
  {
    [SerializeField] private Text _price;
    [SerializeField] private SpriteRenderer _sprite;

    private int Price;
    private Weapon _weapon;

    public override void Interact(PlayerInteract playerInteract)
    {
      if (AppModel.PlayerState().Backpack.GetCoins() < Price)
      {
        return;
      }
      
      AppModel.PlayerState().Backpack.WithdrawResource(ResourceKind.Coins, Price);
      AppModel.PlayerState().Backpack.AddWeapon(_weapon);
      Destroy(gameObject);
    }
    
    public void SetData (Weapon weapon)
    {
      var price = AppModel.WeaponData().GetWeaponInfo(weapon.Type).Price;
      _price.text = price.ToString();
      _sprite.sprite = weapon._uiImage;
      _weapon = weapon;
    }
  }
}