using GamePlay.Player;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay.UI
{
  public class AmmoItem : MonoBehaviour
  {
    [SerializeField] private AmmoKind _kind;
    [SerializeField] private Text _value;
    private 
    void Update()
    {
      _value.text = AppModel.Player().Backpack.Ammo[_kind].ToString();
    }
  }
}