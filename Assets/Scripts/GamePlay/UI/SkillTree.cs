using System;
using System.Collections.Generic;
using GamePlay.Player;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay.UI
{
  public class SkillTree : MonoBehaviour
  {
    [SerializeField] private Text _description;
    [SerializeField] private Text _quantity;
    [SerializeField] private List<SkillTreeBranch> _branches;

    private void Start()
    {
      _description.text = "";
      _quantity.text = AppModel.Player().GetSkillPoints().ToString();

    }

    public void Select(Skill skill)
    {
      var playerState = AppModel.Player();
      playerState.LearnSkill(skill);
      _branches.ForEach(x=>x.UpdateSkillStates());
    }

    public void Hover(Skill skill)
    {
      _description.text = string.Format(skill.Description, skill.Impact);
    }
  }
}