using Cysharp.Threading.Tasks;
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

    private void Start()
    {
      InitHeart().Forget();
    }

    private async UniTask InitHeart()
    {
      await   UniTask.WaitUntil(() => AppModel.PlayerGameObj() != null);
      _player = AppModel.PlayerGameObj().GetComponent<PlayerController>();
      _playerState = AppModel.PlayerState();
      _playerState.OnHealthChanged += ReDraw;
      ReDraw();
    }

    private void ReDraw()
    {
      foreach (Transform child in transform)
        Destroy(child.gameObject);

      var fullContainers = _playerState.CurrentHp;
      var emptyContainers = _playerState.MaxHp - fullContainers;
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