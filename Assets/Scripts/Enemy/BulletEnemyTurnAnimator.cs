using System;
using DefaultNamespace;
using Pathfinding;
using UnityEngine;

namespace Enemy
{
  public class BulletEnemyTurnAnimator : MonoBehaviour
  {
    [SerializeField] private Animator _animator;
    [SerializeField] private AIPath _aiPath;
    private int _currentTurn;
    private bool _previousRunState;
    private bool _isRunning;
    public bool IsDying;
    private bool _isAnimating;

    public bool isAnimating
    {
      get => _isAnimating;
      set
      {
        _currentTurn = 0;
        _isAnimating = value;
      }
    }

    void Update()
    {
      if(IsDying || isAnimating)
        return;;
      Animate();
    }

    private void Animate()
    {
      _isRunning = true;

      var angle = TurnAngle();
      if (angle >= -90 && angle < 10)
      {
        ProcessTurn(EnemyAnimState.downRight);
        return;
      }

      if (angle >= 10 && angle < 90)
      {
        ProcessTurn(EnemyAnimState.upRight);
        return;
      }

      if (angle >= 90 && angle < 170)
      {
        ProcessTurn(EnemyAnimState.upLeft);
        return;
      }

      ProcessTurn(EnemyAnimState.downLeft);
    }

    public float TurnAngle()
    {
      var destination = _aiPath.steeringTarget;
      Vector2 direction = destination - transform.position;
      var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
      return angle;
    }

    private void ProcessTurn(int direction)
    {
      if (_currentTurn == direction)
        return;
      _currentTurn = direction;
      TurnTo(_isRunning);
    }

    protected void TurnTo(bool isRunning)
    {
      if (!isRunning)
      {
        _animator.SetTrigger(_currentTurn);
        return;
      }

      _animator.SetTrigger(_currentTurn);
    }
  }
}