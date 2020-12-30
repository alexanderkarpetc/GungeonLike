using System;
using System.Collections.Generic;
using System.Linq;
using GamePlay.Common;
using GamePlay.Weapons;
using UnityEngine;

namespace GamePlay.Player
{
  public class PlayerState
  {
    private int _maxHp;
    private int _currentHp;
    private int _skillsPoints;

    public event Action OnHealthChanged;
    public event Action OnDamageTake;
    public Weapon Weapon => _weapon;
    public Backpack Backpack = new Backpack();
    public float SpeedMultiplier = 1f;
    public List<Skill> Skills = new List<Skill>();
    private Weapon _weapon;
    public PlayerState()
    {
      _currentHp = 3;
      _maxHp = 3;
    }

    public void SetWeapon(Weapon weapon)
    {
      _weapon = weapon;
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

    public void LearnSkill(Skill skill)
    {
      _skillsPoints--;
      Skills.Add(skill);
    }
    
    public int GetSkillPoints()
    {
      return _skillsPoints;
    }

    public void AddSkillPoint()
    {
      _skillsPoints++;
    }

    public Skill GetNextAvailableSkillOfKind(SkillTreeBranchKind kind)
    {
      var skillsOfKind = StaticData.Skills.Where(x => x.BranchKind == kind).ToList();
      return skillsOfKind.First(x => !Skills.Contains(x));
    }
  }
}