using System;
using System.Collections;
using System.Collections.Generic;
using GamePlay.Player;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay.UI
{
  public class AmmoItem : MonoBehaviour
  {
    [SerializeField] private AmmoKind _kind;
    [SerializeField] private Text _value;
    [SerializeField] private Text _increaseValue;
    [SerializeField] private int _startFontSize;
    [SerializeField] private int _targetFontSize;
    private Coroutine _coroutine;

    private void Start()
    {
      _increaseValue.gameObject.SetActive(false);
      AppModel.Player().Backpack.OnAmmoChange += OnAmmoChange;
    }

    private void OnAmmoChange(Dictionary<AmmoKind, int> ammoBox)
    {
      if (ammoBox.ContainsKey(_kind))
      {
        if (_coroutine != null)
          StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(IncreaseAmmo(ammoBox[_kind]));
      }
    }

    private IEnumerator IncreaseAmmo(int amount)
    {
      _increaseValue.gameObject.SetActive(true);
      _increaseValue.fontSize = _startFontSize;
      _increaseValue.text = $"+{amount}";
      var startTime = Time.time;
      var firstPartEndTime = startTime + 0.2f;
      var secondPartEndTime = firstPartEndTime + 2f;
      var deltaSizePerTime = (_targetFontSize - _startFontSize) / 0.2;
      while (Time.time < firstPartEndTime)
      {
        _increaseValue.fontSize = (int) ((Time.time - startTime) * deltaSizePerTime + _startFontSize);
        yield return null;
      }
      
      while (Time.time < secondPartEndTime)
      {
        _increaseValue.color = new Color(1, 1, 1, ((secondPartEndTime - Time.time) / 2f));
        yield return null;
      }

      _increaseValue.gameObject.SetActive(false);
      _coroutine = null;
      yield return null;
    }

    private void Update()
    {
      _value.text = AppModel.Player().Backpack.Ammo[_kind].ToString();
    }

    private void OnDestroy()
    {
      AppModel.Player().Backpack.OnAmmoChange -= OnAmmoChange;
    }
  }
}