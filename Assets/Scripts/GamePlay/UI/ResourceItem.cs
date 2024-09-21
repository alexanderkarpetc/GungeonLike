using System;
using Cysharp.Threading.Tasks;
using GamePlay.Player;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay.UI
{
  public class ResourceItem : MonoBehaviour
  {
    [SerializeField] private Text _value;
    [SerializeField] private ResourceKind _kind;

    private void Start()
    {
      Subscribe().Forget();
      // todo: uncomment this line
      // _value.text = AppModel.Player().Backpack.Resources[_kind].ToString();
    }

    private async UniTask Subscribe()
    {
      await UniTask.WaitUntil(() => AppModel.PlayerState() != null);
      AppModel.PlayerState().Backpack.OnResourcesChange += OnResourcesChange;
    }
    
    private void OnResourcesChange(ResourceKind kind, int value)
    {
      if (kind == _kind)
        _value.text = AppModel.PlayerState().Backpack.Resources[_kind].ToString();
    }
    private void OnDestroy()
    {
      AppModel.PlayerState().Backpack.OnResourcesChange -= OnResourcesChange;
    }
  }
}