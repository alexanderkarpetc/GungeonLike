using System.Collections.Generic;
using System.Linq;
using GamePlay.Common;
using GamePlay.Player;
using UnityEngine;

namespace GamePlay.UI
{
  public class SkillTreeBranch : MonoBehaviour
  {
    [SerializeField] private GameObject _obj;
    [SerializeField] private GameObject _separator;
    [SerializeField] private SkillTreeBranchKind _branchKind;
    [SerializeField] private SkillTree _skillTree;

    private List<SkillItem> _items = new List<SkillItem>();
    void Start()
    {
      var skills = StaticData.Skills.Where(x => x.BranchKind == _branchKind);
      var index = 1;
      foreach (var skillInfo in skills)
      {
        var skillObj = Instantiate(_obj, transform);
        var skillItem = skillObj.GetComponent<SkillItem>();
        skillItem.Init(skillInfo, _skillTree);
        _items.Add(skillItem);
        if (skills.Last() != skillInfo)
        {
          var separator = Instantiate(_separator, transform);
          separator.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -index*126);
          index++;
        }
      }
    }

    public void UpdateSkillStates()
    {
      _items.ForEach(x=>x.UpdateState());
    }
  }
}