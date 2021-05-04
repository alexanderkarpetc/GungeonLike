using GamePlay.Common;
using GamePlay.Player;
using GamePlay.Weapons;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay.Level
{
  public class ShopItemView : MonoBehaviour
  {
    [SerializeField] private Text _price;
    [SerializeField] private SpriteRenderer _sprite;

    private ShopItemInteractable _interactable;

    public void SetData (Weapon weapon)
    {
      var price = AppModel.WeaponData().GetWeaponInfo(weapon.Type).Price;
      _price.text = price.ToString();
      _sprite.sprite = weapon._uiImage;
      _interactable = new ShopItemInteractable
      {
        _price = price,
        _weapon = weapon
      };
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
      if (other.CompareTag("Player"))
      {
        other.GetComponent<PlayerInteract>().Interactable = _interactable;
      }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
      if (other.CompareTag("Player"))
      {
        other.GetComponent<PlayerInteract>().Interactable = null;
      }
    }
  }
}