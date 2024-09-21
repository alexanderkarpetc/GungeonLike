using System;
using System.Collections.Generic;
using System.Linq;
using GamePlay.Common;
using GamePlay.Enemy;
using GamePlay.Weapons;
using Unity.Netcode;
using UnityEngine;

namespace GamePlay.Player
{
  public class PlayerState : NetworkBehaviour
  {
    public int CurrentHp;
    public int MaxHp;

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

    private void Awake()
    {
      AppModel.SetPlayerState(this);
    }

    public void SetWeapon(Weapon weapon)
    {
      _weapon = weapon;
    }

    public void Heal()
    {
      CurrentHp = Mathf.Clamp(CurrentHp + 1, 0, MaxHp);
      OnHealthChanged.NullSafeInvoke();
    }
    
    public void DealDamage()
    {
      CurrentHp--;
      OnHealthChanged.NullSafeInvoke();
      OnDamageTake.NullSafeInvoke();
    }
    
    public void IncreaseMaxHp()
    {
      MaxHp++;
      OnHealthChanged.NullSafeInvoke();
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