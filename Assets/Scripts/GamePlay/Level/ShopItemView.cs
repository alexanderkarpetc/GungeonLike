using System.Linq;
using GamePlay.Player;
using GamePlay.Weapons;
using Unity.Netcode;
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
      AddWeaponServerRpc();
      Destroy(gameObject);
    }

    [ServerRpc]
    private void AddWeaponServerRpc()
    {
      AddWeaponClientRpc();
    }

    [ClientRpc]
    private void AddWeaponClientRpc()
    {
      AppModel.PlayerState(OwnerClientId).Backpack.AddWeapon(_weapon);
    }

    [ServerRpc]
    public void SetDataServerRpc(WeaponType type)
    {
      SetDataClientRpc(type);
    }
    
    [ClientRpc]
    private void SetDataClientRpc (WeaponType type)
    {
      var weapon = AppModel.DropManager().AllGuns.First(gun => gun.Type == type);
      var price = AppModel.WeaponData().GetWeaponInfo(weapon.Type).Price;
      _price.text = price.ToString();
      _sprite.sprite = weapon._uiImage;
      _weapon = weapon;
    }
  }
}