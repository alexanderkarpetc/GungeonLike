using System.Collections;
using GamePlay.UI;
using Popups;
using UnityEngine;

namespace GamePlay.Player
{
  public class HudController : MonoBehaviour
  {
    [SerializeField] private GameObject _dmgPanel;
    [SerializeField] private float _blinkDuration;
    [SerializeField] private GameObject _console;
    [SerializeField] private GameObject _devPanel;

    private void Start()
    {
      AppModel.Player().OnDamageTake += ScreenBlink;
      _console.SetActive(false);
      _dmgPanel.SetActive(false);
      _devPanel.SetActive(false);
    }
    public GameObject DevPanel => _devPanel;

    private void Update()
    {
      if (Input.GetKeyDown(KeyCode.BackQuote))
      {
        _console.SetActive(!_console.activeSelf);
      }
      if (Input.GetKeyDown(KeyCode.Escape))
      {
        if (PopupManager.Instance.IsShown<SkillTree>())
          PopupManager.Instance.HidePopup<SkillTree>();
        else
          PopupManager.Instance.ShowPopup<SkillTree>();
      }
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