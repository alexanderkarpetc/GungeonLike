using System;
using DefaultNamespace;
using Pathfinding;
using UnityEngine;

namespace Enemy
{
  public class BulletEnemyTurnAnimator : MonoBehaviour
  {
    [SerializeField] private Animator _animator;
    [SerializeField] private AIDestinationSetter _destinationSetter;
    [SerializeField] private AIPath _aiPath;
    private int _currentTurn;
    private bool _previousRunState;
    private bool _isRunning;

    private void Start()
    {
      _destinationSetter.target = GameObject.Find("Player").transform;
    }

    void Update()
    {
      Animate();
    }

    private void Animate()
    {
      var destination = _aiPath.steeringTarget;
      _isRunning = true;
      Vector2 direction = destination - transform.position;
      var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
      Debug.Log(angle);
      if (angle >= -75 && angle < 25)
      {
        ProcessTurn(EnemyAnimState.downRight, 1);
        return;
      }

      if (angle >= 25 && angle < 70)
      {
        ProcessTurn(EnemyAnimState.upRight, 1);
        return;
      }

      if (angle >= 70 && angle < 110)
      {
        ProcessTurn(EnemyAnimState.downLeft, 1);
        return;
      }

      if (angle >= 110 && angle < 155)
      {
        ProcessTurn(EnemyAnimState.upLeft, -1);
        return;
      }
    }

    private void ProcessTurn(int direction, int scale)
    {
      if (_currentTurn == direction && _previousRunState == _isRunning)
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