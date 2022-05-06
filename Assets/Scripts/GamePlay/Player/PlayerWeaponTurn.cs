using GamePlay.Common;
using UnityEngine;

namespace GamePlay.Player
{
  public class PlayerWeaponTurn : WeaponTurn
  {
    private Animator _playerAnimator;

    protected override void TurnGun()
    {
      Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
      Angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
      var rot = Quaternion.AngleAxis(Mathf.Abs(Angle) < 90 ? Angle : - 180 + Angle, Vector3.forward);
      transform.rotation = rot;
    }

    protected override void OnStart()
    {
      _playerAnimator = AppModel.PlayerGameObj().GetComponent<Animator>();
    }
    
    protected override void ChangeSortingOrder()
    {
      var state = _playerAnimator.GetCurrentAnimatorStateInfo(0);

      if (state.IsName(PlayerAnimState.IdleDown) || state.IsName(PlayerAnimState.RunDown) ||
          state.IsName(PlayerAnimState.IdleDownRight) || state.IsName(PlayerAnimState.RunDownRight))
      {
        Weapon.Body.sortingOrder = 2;
        _leftHandRenderer.sortingOrder = 3;
        _rightHandRenderer.sortingOrder = 3;
      }
      else if (state.IsName(PlayerAnimState.IdleUp) || state.IsName(PlayerAnimState.RunUp) ||
               state.IsName(PlayerAnimState.IdleUpRight) || state.IsName(PlayerAnimState.RunUpRight))
      {
        Weapon.Body.sortingOrder = -2;
        _leftHandRenderer.sortingOrder = -3;
        _rightHandRenderer.sortingOrder = -3;
      }
    }
  }
}