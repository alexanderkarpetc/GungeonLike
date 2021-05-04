using System;
using System.Collections.Generic;
using System.IO;
using GamePlay;
using GamePlay.Common;
using GamePlay.Player;
using GamePlay.Weapons;

namespace Import
{
  public static class BalanceLoader
  {
    public static void LoadBalance()
    {
      ReadSkills();
      ReadLevels();
      ReadWeapons();
    }

    private static void ReadSkills()
    {
      var lines = File.ReadAllLines("Assets/Balance/Skills.csv");
      var skills = new List<Skill>();
      for (var i = 1; i < lines.Length; i++)
      {
        var skillLine = lines[i].Split(';');
        var skill = new Skill
        {
          Kind = (SkillKind) Enum.Parse(typeof(SkillKind), skillLine[0]),
          Impact = float.Parse(skillLine[1]),
          Description = skillLine[2],
          Icon = skillLine[3],
          BranchKind = (SkillTreeBranchKind) Enum.Parse(typeof(SkillTreeBranchKind), skillLine[4]),
        };
        skills.Add(skill);
      }

      StaticData.Skills = skills;
    }
    
    private static void ReadLevels()
    {
      var lines = File.ReadAllLines("Assets/Balance/Levels.csv");
      var exps = new List<int>();
      for (var i = 1; i < lines.Length; i++)
      {
        exps.Add(int.Parse(lines[i]));
      }

      StaticData.Levels = exps;
    }  
    
    private static void ReadWeapons()
    {
      var lines = File.ReadAllLines("Assets/Balance/Weapons.csv");
      var infos = new Dictionary<WeaponType, WeaponInfo>();
      for (var i = 1; i < lines.Length; i++)
      {
        var weaponLine = lines[i].Split(';');
        var weaponInfo = new WeaponInfo()
        {
          Id = int.Parse(weaponLine[0]),
          Type = (WeaponType) Enum.Parse(typeof(WeaponType), weaponLine[1]),
          Damage = float.Parse(weaponLine[2]),
          Price = int.Parse(weaponLine[3]),
        };
        infos.Add(weaponInfo.Type, weaponInfo);
      }

      AppModel.WeaponData().WeaponInfos = infos;
    }
  }
}