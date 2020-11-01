using UnityEngine;

namespace GamePlay.Enemy
{
  public class EnemyWeaponTurn : WeaponTurn
  {
    [SerializeField] private Animator _animator;
    private GameObject _player;

    protected override void OnStart()
    {
      _player = GameObject.Find("Player");
    }

    protected override void TurnGun()
    {
      Vector2 direction = _player.transform.position - transform.position;
      Angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
      var rot = Quaternion.AngleAxis(Mathf.Abs(Angle) < 90 ? Angle : - 180 + Angle, Vector3.forward);
      transform.rotation = rot;
    }
    protected override void ChangeSortingOrder()
    {
      var state = _animator.GetCurrentAnimatorStateInfo(0);

      if (state.IsName(EnemyAnimState.UpRight) || state.IsName(EnemyAnimState.UpLeft) ||
          state.IsName(EnemyAnimState.IdleBack) || state.IsName(EnemyAnimState.HitLeftUp)|| state.IsName(EnemyAnimState.HitRightUp))
      {
        _body.sortingOrder = 1;
        _rightHandSprite.sortingOrder = 2;
        _leftHandSprite.sortingOrder = 2;
      }
      else
      {
        _body.sortingOrder = -2;
        _rightHandSprite.sortingOrder = -2;
        _leftHandSprite.sortingOrder = -2;
      }
    }
    protected override void MoveHands()
    {
      if (Mathf.Abs(Angle) < 90)
      {
        gameObject.transform.position = _rightHand.position;
        SpriteUtil.SetXScale(gameObject, 1);
        Weapon.IsInverted = false;
      }
      else
      {
        gameObject.transform.position = _leftHand.position;
        SpriteUtil.SetXScale(gameObject, -1);
        Weapon.IsInverted = true;
      }
    }
  }
}