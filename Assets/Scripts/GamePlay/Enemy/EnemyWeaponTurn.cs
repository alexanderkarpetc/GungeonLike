using UnityEngine;

namespace GamePlay.Enemy
{
  public class EnemyWeaponTurn : WeaponTurn
  {
    [SerializeField] private Animator _animator;
    private GameObject _player;
    private EnemyController _controller;

    protected override void OnStart()
    {
      _player = AppModel.PlayerGameObj();
      _controller = GetComponent<EnemyController>();
    }

    protected override void TurnGun()
    {
      var lookingPos = _controller.CurrentTarget ?? _player.transform.position;
      Vector2 direction = lookingPos - _weaponSlot.transform.position;
      Angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
      var rot = Quaternion.AngleAxis(Mathf.Abs(Angle) < 90 ? Angle : - 180 + Angle, Vector3.forward);
      _weaponSlot.transform.rotation = rot;
    }

    protected override void ChangeSortingOrder()
    {
      var state = _animator.GetCurrentAnimatorStateInfo(0);

      if (state.IsName(EnemyAnimState.UpRight) || state.IsName(EnemyAnimState.UpLeft) ||
          state.IsName(EnemyAnimState.IdleBack) || state.IsName(EnemyAnimState.HitLeftUp)|| state.IsName(EnemyAnimState.HitRightUp))
      {
        Weapon.Body.sortingOrder = -2;
        _leftHandRenderer.sortingOrder = -3;
        _rightHandRenderer.sortingOrder = -3;
      }
      else
      {
        Weapon.Body.sortingOrder = 2;
        _leftHandRenderer.sortingOrder = 3;
        _rightHandRenderer.sortingOrder = 3;
      }
    }
  }
}