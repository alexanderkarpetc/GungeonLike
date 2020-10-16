using DefaultNamespace;
using UnityEngine;

namespace Player
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
      _playerAnimator = GameObject.Find("Player").GetComponent<Animator>();
    }

    protected override void MoveHands()
    {

      if (Mathf.Abs(Angle) < 90)
      {
        _leftHand.localPosition = _leftHandPos;
        gameObject.transform.position = _leftHand.position;
        SpriteUtil.SetXScale(gameObject, 1);
        Weapon.IsInverted = false;
        if(_secondaryHandPos != null)
          _rightHand.position = _secondaryHandPos.position;
      }
      else
      {
        _rightHand.localPosition = _rightHandPos;
        gameObject.transform.position = _rightHand.position;
        SpriteUtil.SetXScale(gameObject, -1);
        Weapon.IsInverted = true;
        if(_secondaryHandPos != null)
          _leftHand.position = _secondaryHandPos.position;
      }
    }
    
    protected override void ChangeSortingOrder()
    {
      var state = _playerAnimator.GetCurrentAnimatorStateInfo(0);

      if (state.IsName(PlayerAnimState.IdleDown) || state.IsName(PlayerAnimState.RunDown) ||
          state.IsName(PlayerAnimState.IdleDownRight) || state.IsName(PlayerAnimState.RunDownRight))
      {
        _body.sortingOrder = 1;
        _rightHandSprite.sortingOrder = 2;
        _leftHandSprite.sortingOrder = 2;
      }
      else if (state.IsName(PlayerAnimState.IdleUp) || state.IsName(PlayerAnimState.RunUp) ||
               state.IsName(PlayerAnimState.IdleUpRight) || state.IsName(PlayerAnimState.RunUpRight))
      {
        _body.sortingOrder = -2;
        _rightHandSprite.sortingOrder = -2;
        _leftHandSprite.sortingOrder = -2;
      }
    }
  }
}