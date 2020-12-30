using System;
using System.Collections.Generic;
using System.IO;
using GamePlay.Common;
using GamePlay.Player;

namespace Import
{
  public static class BalanceLoader
  {
    public static void LoadBalance()
    {
      ReadSkills();
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
  }
}