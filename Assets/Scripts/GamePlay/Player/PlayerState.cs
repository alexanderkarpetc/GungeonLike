using System;
using System.Collections.Generic;
using System.Linq;
using GamePlay.Common;
using GamePlay.Enemy;
using GamePlay.Weapons;
using UnityEngine;

namespace GamePlay.Player
{
  public class PlayerState
  {
    private int _maxHp;
    private int _currentHp;
    private int _skillsPoints;
    private int _level = 1;
    private int _exp = 0;

    public event Action OnHealthChanged;
    public event Action OnDamageTake;
    public Weapon Weapon => _weapon;
    public Backpack Backpack = new Backpack();
    public float SpeedMultiplier = 1f;
    public List<Skill> Skills = new List<Skill>();
    public int Level => _level;
    public int Exp => _exp;
    private Weapon _weapon;
    public event Action OnSkillLearned;

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
      OnSkillLearned?.Invoke();
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

    private void LevelUp()
    {
      _level++;
      _skillsPoints++;
    }

    public void AddExp(int exp)
    {
      _exp += exp;
      if (_exp >= StaticData.RequiredXp(_level))
      {
        _exp -= StaticData.RequiredXp(_level);
        LevelUp();
      }
    }

    public void AddExp(EnemyType type)
    {
      AddExp(100);
    }
  }
}