using System;
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
      AppModel.Player().Backpack.OnResourcesChange += OnResourcesChange;
      _value.text = AppModel.Player().Backpack.Resources[_kind].ToString();
    }

    private void OnResourcesChange(ResourceKind kind, int value)
    {
      if (kind == _kind)
        _value.text = AppModel.Player().Backpack.Resources[_kind].ToString();
    }
    private void OnDestroy()
    {
      AppModel.Player().Backpack.OnResourcesChange -= OnResourcesChange;
    }
  }
}