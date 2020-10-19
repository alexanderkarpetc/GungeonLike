using System;
using UnityEngine;

namespace Player
{
  public class PlayerState
  {
    private int _maxHp;
    private int _currentHp;
    public event Action OnHealthChanged;
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