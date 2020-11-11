using System.Collections;
using System.Collections.Generic;
using GamePlay.Weapons;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay.Player
{
  public class WeaponHudElement : MonoBehaviour
  {
    [SerializeField] private Image _weapon;
    [SerializeField] private Image _magazine;

    private Weapon _currentWeapon;
    private bool _reloadStarted;
    private Coroutine _reload;

    private void Update()
    {
      var weapon = AppModel.Player().Weapon;
      if (_currentWeapon != weapon)
      {
        if (_reload != null)
        {
          _reloadStarted = false;
          StopCoroutine(_reload);
        }
        _currentWeapon = weapon;
      }
      _weapon.sprite = _currentWeapon._uiImage;
      if (!_currentWeapon.reloading)
        _magazine.fillAmount = (float) _currentWeapon.State.bulletsLeft / _currentWeapon.MagazineSize;
      else
      {
        if (_reloadStarted)
          return;
        _reload = StartCoroutine(Reload());
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

      _reload = null;
      _reloadStarted = false;
    }
  }
}