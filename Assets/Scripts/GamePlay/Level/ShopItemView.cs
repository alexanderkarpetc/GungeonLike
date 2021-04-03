using GamePlay.Player;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay.Level
{
  public class ShopItemView : MonoBehaviour
  {
    [SerializeField] private Text _price;

    private ShopItemInteractable _interactable;

    void Start()
    {
      _interactable = new ShopItemInteractable();
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