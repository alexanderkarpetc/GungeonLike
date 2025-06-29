using System.Linq;
using GamePlay.Player;
using GamePlay.Weapons;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay.Level
{
  public class PickableItemView : Interactable
  {
    [SerializeField] private Text _price;
    [SerializeField] private SpriteRenderer _sprite;
    public Weapon PredefinedWeapon;// for testing only

    public NetworkVariable<WeaponType> WeaponType = new();
    public NetworkVariable<bool> HasWeapon = new();

    private int Price;
    private Weapon _weapon;
    private WeaponType? _preparedWeaponType;

    public override void OnNetworkSpawn()
    {
      // for testing only
      if (PredefinedWeapon != null)
      {
        HasWeapon.Value = true;
        WeaponType.Value = PredefinedWeapon.Type;
      }

      if (_preparedWeaponType != null)
      {
        WeaponType.Value = _preparedWeaponType.Value;
        HasWeapon.Value = true;
      }

      if (HasWeapon.Value)
      {
        var weapon = AppModel.DropManager().AllGuns.First(gun => gun.Type == WeaponType.Value);
        if (_price != null)
        {
          var price = AppModel.WeaponData().GetWeaponInfo(weapon.Type).Price;
          _price.text = price.ToString();
        }

        _sprite.sprite = weapon._uiImage;
        _weapon = weapon;
      }
    }

    public void Prepare(WeaponType weaponType)
    {
      _preparedWeaponType = weaponType;
    }

    public override void Interact(PlayerInteract playerInteract)
    {
      if (Price > 0 && AppModel.PlayerState().Backpack.GetCoins() < Price)
      {
        return;
      }
      
      AppModel.PlayerState().Backpack.WithdrawResource(ResourceKind.Coins, Price);
      AddWeaponServerRpc(playerInteract.OwnerClientId);
    }

    [ServerRpc (RequireOwnership = false)]
    private void AddWeaponServerRpc(ulong ownerClientId)
    {
      AddWeaponClientRpc(ownerClientId);
      Destroy(gameObject);
    }

    [ClientRpc]
    private void AddWeaponClientRpc(ulong ownerClientId)
    {
      AppModel.PlayerState(ownerClientId).AddWeapon(_weapon);
    }
  }
}