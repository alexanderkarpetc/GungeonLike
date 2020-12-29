using System.Collections;
using UnityEngine;

namespace GamePlay.Player
{
  public class HudController : MonoBehaviour
  {
    [SerializeField] private GameObject _dmgPanel;
    [SerializeField] private float _blinkDuration;
    [SerializeField] private GameObject _console;
    [SerializeField] private GameObject _skillTree;

    private void Start()
    {
      AppModel.Player().OnDamageTake += ScreenBlink;
    }

    private void Update()
    {
      if (Input.GetKeyDown(KeyCode.BackQuote))
      {
        _console.SetActive(!_console.activeSelf);
      }
    }

    public void OpenSkillTree()
    {
      _skillTree.SetActive(true);
    }
    
    public void CloseSkillTree()
    {
      _skillTree.SetActive(true);
    }

    private void ScreenBlink()
    {
      StartCoroutine(DoBlink());
    }

    private IEnumerator DoBlink()
    {
      _dmgPanel.SetActive(true);
      yield return new WaitForSeconds(_blinkDuration);
      _dmgPanel.SetActive(false);
    }
  }
}