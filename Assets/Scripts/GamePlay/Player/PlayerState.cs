using System;
using UnityEngine;

namespace GamePlay.Player
{
  public class PlayerState
  {
    private int _maxHp;
    private int _currentHp;
    public event Action OnHealthChanged;
    public event Action OnDamageTake;
    public PlayerState()
    {
      _currentHp = 3;
      _maxHp = 3;
    }

    public int GetHp()
    {
      return _currentHp;
    }

    public void Heal()
    {
      _currentHp = Mathf.Clamp(_currentHp + 1, 0, _maxHp);
      OnHealthChanged.NullSafeInvoke();
    }
    
    public void DealDamage()
    {
      _currentHp--;
      OnHealthChanged.NullSafeInvoke();
      OnDamageTake.NullSafeInvoke();
    }
    
    public void IncreaseMaxHp()
    {
      _maxHp++;
      OnHealthChanged.NullSafeInvoke();
    }

    public int GetMaxHp()
    {
      return _maxHp;
    }
  }
}