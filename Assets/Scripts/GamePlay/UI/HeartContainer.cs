using DefaultNamespace;
using GamePlay.Player;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay
{
  public class HeartContainer : MonoBehaviour
  {
    [SerializeField] private GameObject heart;
    [SerializeField] private Sprite _full;
    [SerializeField] private Sprite _empty;
    private PlayerController _player;
    private PlayerState _playerState;

    void Start()
    {
      _player = AppModel.PlayerGameObj().GetComponent<PlayerController>();
      _playerState = AppModel.Player();
      _playerState.OnHealthChanged += ReDraw;
      ReDraw();
    }

    private void ReDraw()
    {
      foreach (Transform child in transform)
        Destroy(child.gameObject);

      var fullContainers = _playerState.GetHp();
      var emptyContainers = _playerState.GetMaxHp() - fullContainers;
      for (var i = 0; i < fullContainers; i++)
      {
        var go = Instantiate(heart, transform);
        go.GetComponent<Image>().sprite = _full;
      }

      for (var i = 0; i < emptyContainers; i++)
      {
        var go = Instantiate(heart, transform);
        go.GetComponent<Image>().sprite = _empty;
      }
    }
  }
}