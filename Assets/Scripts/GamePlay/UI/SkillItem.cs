using System.Linq;
using GamePlay.Player;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay.UI
{
  public class SkillItem : MonoBehaviour
  {
    private static Color _picked = new Color(0.345098f, 0.8235294f, 0.17f);
    private enum State
    {
      Disabled =1,
      Enabled =2,
      Picked = 3
    }

    [SerializeField] private Image _image;
    [SerializeField] private Image _border;
    [SerializeField] private GameObject _available;
    private Skill _skill;
    private State _state;
    private SkillTree _skillTree;
    

    public void Init(Skill skill, SkillTree skillTree)
    {
      _skillTree = skillTree;
      _skill = skill;
      _image.sprite = Resources.Load<Sprite>("Prefabs/UI/UiIcons/" + _skill.Icon);
      UpdateState();
    }

    public void UpdateState()
    {
      var skills = AppModel.Player().Skills.Where(x=>x.BranchKind == _skill.BranchKind).ToList();
      if (skills.Contains(_skill))
      {
        _state = State.Picked;
      }
      else if (AppModel.Player().GetNextAvailableSkillOfKind(_skill.BranchKind) == _skill)
      {
        _state = State.Enabled;
      }
      else
      {
        _state = State.Disabled;
      }

      UpdateView();
    }

    private void UpdateView()
    {
      if (_state == State.Disabled || _state == State.Enabled)
      {
        var tempColor = _image.color;
        tempColor.a = 0.5f;
        _image.color = tempColor;
      }
      else
      {
        var tempColor = _image.color;
        tempColor.a = 1;
        _image.color = tempColor;
      }
      _available.SetActive(_state == State.Enabled);
      if (_state == State.Picked)
      {
        _border.color = _picked;
      }
    }

    public void Select()
    {
      if(_state != State.Enabled)
        return;
      
      _skillTree.Select(_skill);
      UpdateState();
    }
    
    public void Hover()
    {
      _skillTree.Hover(_skill);
    }
  }
}