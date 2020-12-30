using System;
using System.Collections.Generic;
using GamePlay.Common;
using GamePlay.Player;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay.UI
{
  public class SkillTree : MonoBehaviour
  {
    [SerializeField] private Text _description;
    [SerializeField] private Text _quantity;
    [SerializeField] private Text _level;
    [SerializeField] private RectTransform _levelProgress;
    [SerializeField] private List<SkillTreeBranch> _branches;

    private float _progressMax = 800f;
    private void Start()
    {
      _description.text = "";
      Redraw();
    }

    public void Select(Skill skill)
    {
      var playerState = AppModel.Player();
      playerState.LearnSkill(skill);
      _branches.ForEach(x=>x.UpdateSkillStates());
      Redraw();
    }

    private void Redraw()
    {
      var playerState = AppModel.Player();
      _quantity.text = playerState.GetSkillPoints().ToString();
      _level.text = $"Current Level: {playerState.Level}";
      _levelProgress.sizeDelta = new Vector2(_levelProgress.sizeDelta.x,
        playerState.Exp / StaticData.RequiredXp(playerState.Level) * _progressMax);
    }
    
    public void Hover(Skill skill)
    {
      _description.text = string.Format(skill.Description, skill.Impact);
    }
  }
}