using UnityEngine;

namespace Player
{
  public class PlayerWeaponTurn : WeaponTurn
  {
    protected override void TurnGun()
    {
      base.TurnGun();
    }

    protected override void ChangeSortingOrder(AnimatorStateInfo state)
    {
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