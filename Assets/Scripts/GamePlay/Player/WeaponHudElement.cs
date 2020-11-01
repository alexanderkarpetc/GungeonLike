using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay.Player
{
  public class WeaponHudElement : MonoBehaviour
  {
    [SerializeField] private Image _weapon;
    [SerializeField] private Image _magazine;

    private bool _reloadStarted;

    private void Update()
    {
      var weapon = AppModel.Player().Weapon;
      _weapon.sprite = weapon._uiImage;
      if (!weapon.reloading)
        _magazine.fillAmount = (float) weapon.bulletsLeft / weapon.MagazineSize;
      else
      {
        if (_reloadStarted)
          return;
        StartCoroutine(Reload());
      }
    }

    private IEnumerator Reload()
    {
      _reloadStarted = true;
      var weapon = AppModel.Player().Weapon;

      var startTime = Time.time;
      while (Time.time < startTime + weapon.reloadingTime)
      {
        _magazine.fillAmount = 1 - (startTime + weapon.reloadingTime - Time.time) / weapon.reloadingTime;
        yield return null;
      }

      _reloadStarted = false;
    }
  }
}