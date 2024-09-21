using System.Collections;
using Cysharp.Threading.Tasks;
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
      Subscribe().Forget();
      _console.SetActive(false);
      _dmgPanel.SetActive(false);
      _devPanel.SetActive(false);
    }

    private async UniTask Subscribe()
    {
      await UniTask.WaitUntil(() => AppModel.PlayerState() != null);
      AppModel.PlayerState().OnDamageTake += ScreenBlink;
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