using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
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
    private CancellationTokenSource _cts;

    private void Start()
    {
      _increaseValue.gameObject.SetActive(false);
      Subscribe().Forget();
    }

    private async UniTask Subscribe()
    {
      await UniTask.WaitUntil(() => AppModel.PlayerState() != null);
      AppModel.PlayerState().Backpack.OnAmmoChange += OnAmmoChange;
    }

    private void OnAmmoChange(Dictionary<AmmoKind, int> ammoBox)
    {
      if (ammoBox.ContainsKey(_kind))
      {
        _cts?.Cancel();
        _cts = new CancellationTokenSource();
        IncreaseAmmo(ammoBox[_kind], _cts.Token).Forget();
      }
    }

    private async UniTaskVoid IncreaseAmmo(int amount, CancellationToken token)
    {
      _increaseValue.gameObject.SetActive(true);
      _increaseValue.fontSize = _startFontSize;
      _increaseValue.text = $"+{amount}";
      var startTime = Time.time;
      var firstPartEndTime = startTime + 0.2f;
      var secondPartEndTime = firstPartEndTime + 2f;
      var deltaSizePerTime = (_targetFontSize - _startFontSize) / 0.2f;

      while (Time.time < firstPartEndTime)
      {
        if (token.IsCancellationRequested) return;
        _increaseValue.fontSize = (int)((Time.time - startTime) * deltaSizePerTime + _startFontSize);
        await UniTask.Yield(PlayerLoopTiming.Update);
      }

      while (Time.time < secondPartEndTime)
      {
        if (token.IsCancellationRequested) return;
        _increaseValue.color = new Color(1, 1, 1, ((secondPartEndTime - Time.time) / 2f));
        await UniTask.Yield(PlayerLoopTiming.Update);
      }

      _increaseValue.gameObject.SetActive(false);
      _cts = null;
    }

    private void Update()
    {
      if (AppModel.PlayerState()?.Backpack?.IsInitialized == true)
      {
        _value.text = AppModel.PlayerState().Backpack.Ammo[_kind].ToString();
      }
    }

    private void OnDestroy()
    {
      AppModel.PlayerState().Backpack.OnAmmoChange -= OnAmmoChange;
      _cts?.Cancel();
    }
  }
}