using System;
using System.Collections;
using Pathfinding;
using UnityEngine;

namespace GamePlay.Enemy
{
  public class BulletEnemyAnimator : EnemyAnimator
  {
    [SerializeField] private AIPath _aiPath;

    private bool _previousRunState;
    private bool _isAnimating;

    private bool isAnimating
    {
      get => _isAnimating;
      set
      {
        //todo: get rid of this?
        _currentAnim = 0;
        _isAnimating = value;
      }
    }

    public override void Hit(float hitAnimDuration)
    {
      StartCoroutine(ShowHit(hitAnimDuration));
    }

    private IEnumerator ShowHit(float hitAnimDuration)
    {
      isAnimating = true;
      var angle = TurnAngle(_controller.CurrentTarget ?? _aiPath.steeringTarget);
      if (angle >= -90 && angle < 10)
      {
        _animator.SetTrigger(EnemyAnimState.hitRightDown);
      }

      else if (angle >= 10 && angle < 90)
      {
        _animator.SetTrigger(EnemyAnimState.hitRightUp);
      }

      else if (angle >= 90 && angle < 170)
      {
        _animator.SetTrigger(EnemyAnimState.hitLeftUp);
      }

      else
      {
        _animator.SetTrigger(EnemyAnimState.hitLeftDown);
      }
      
      yield return new WaitForSeconds(hitAnimDuration);
      isAnimating = false;
    }

    private void Update()
    {
      if(isAnimating)
        return;
      Animate();
    }

    private void Animate()
    {
      var angle = TurnAngle(_controller.CurrentTarget ?? _aiPath.steeringTarget);
      if (_aiPath.maxSpeed == 0 && angle is >= 10 and < 170)
      {
        ProcessAnimation(EnemyAnimState.idleBack);
        return;
      }

      if (_aiPath.maxSpeed == 0)
      {
        ProcessAnimation(EnemyAnimState.idle);
        return;
      }
      
      if (angle >= -90 && angle < 10)
      {
        ProcessAnimation(EnemyAnimState.downRight);
        return;
      }

      if (angle >= 10 && angle < 90)
      {
        ProcessAnimation(EnemyAnimState.upRight);
        return;
      }

      if (angle >= 90 && angle < 170)
      {
        ProcessAnimation(EnemyAnimState.upLeft);
        return;
      }

      ProcessAnimation(EnemyAnimState.downLeft);
    }
  }
}