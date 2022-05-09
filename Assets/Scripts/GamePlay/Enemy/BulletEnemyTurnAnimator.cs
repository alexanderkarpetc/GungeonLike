using System;
using Pathfinding;
using UnityEngine;

namespace GamePlay.Enemy
{
  public class BulletEnemyTurnAnimator : MonoBehaviour
  {
    [SerializeField] private Animator _animator;
    [SerializeField] private AIPath _aiPath;
    private EnemyController _controller;

    private int _currentTurn;
    private bool _previousRunState;
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

    private void Start()
    {
      _controller = GetComponent<EnemyController>();
    }

    void Update()
    {
      if(IsDying || isAnimating)
        return;;
      Animate();
    }

    private void Animate()
    {
      var angle = TurnAngle();
      if (_aiPath.maxSpeed == 0 && angle is >= 10 and < 170)
      {
        ProcessTurn(EnemyAnimState.idleBack);
        return;
      }

      if (_aiPath.maxSpeed == 0)
      {
        ProcessTurn(EnemyAnimState.idle);
        return;
      }
      
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
      var destination = _controller.CurrentTarget ?? _aiPath.steeringTarget;
      Vector2 direction = destination - transform.position;
      var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
      return angle;
    }

    private void ProcessTurn(int direction)
    {
      if (_currentTurn == direction)
        return;
      _currentTurn = direction;
      TurnTo();
    }

    protected void TurnTo()
    {
      _animator.SetTrigger(_currentTurn);
    }
  }
}