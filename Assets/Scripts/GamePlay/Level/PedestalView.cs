using UnityEngine;

namespace GamePlay.Level
{
  [RequireComponent(typeof(PickableItemView))]
  public class PedestalView : MonoBehaviour
  {
    [SerializeField] private SpriteRenderer _itemSprite;

    private void Start()
    {
      _itemSprite.sprite = GetComponent<PickableItemView>().Weapon._uiImage;
    }
  }
}