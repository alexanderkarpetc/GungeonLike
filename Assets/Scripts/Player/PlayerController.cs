using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UIElements;

namespace Player
{
  public class PlayerController : MonoBehaviour
  {
    [SerializeField] private Animator _animator;
    [SerializeField] private float Speed;
    [SerializeField] private GameObject _body;
    private int _verticalMove;
    private int _horizontalMove;
    private int _currentTurn;
    private bool _previousRunState;

    private void Update()
    {
      ReadInput();
      Animate();
    }

    private void ReadInput()
    {
      var a = Input.GetKey(KeyCode.A) ? 1 : 0;
      var d = Input.GetKey(KeyCode.D) ? 1 : 0;
      _horizontalMove = d - a;
      var w = Input.GetKey(KeyCode.W) ? 1 : 0;
      var s = Input.GetKey(KeyCode.S) ? 1 : 0;
      _verticalMove = w - s;
      Move();
    }

    private void Move()
    {
      transform.Translate(Time.deltaTime * Speed * new Vector2(_horizontalMove, _verticalMove).normalized);
    }
    
    private void Animate()
    {
      Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
      var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
      if (angle >= -75 && angle < 25)
      {
        ProcessPlayerTurn(PlayerAnimState.DownRight, 1);
        return;
      }
      if (angle >= 25 && angle < 70)
      {
        ProcessPlayerTurn(PlayerAnimState.UpRight, 1);
        return;
      }
      if (angle >= 70 && angle < 110)
      {
        ProcessPlayerTurn(PlayerAnimState.Up, 1);
        return;
      }
      if (angle >= 110 && angle < 155)
      {
        ProcessPlayerTurn(PlayerAnimState.UpRight, -1);
        return;
      }
      
      if (angle > -135 && angle < -75)
      {
        ProcessPlayerTurn(PlayerAnimState.Down, 1);
        return;
      }
      
      if (Mathf.Abs(angle) >= 135)
      {
        ProcessPlayerTurn(PlayerAnimState.DownRight, -1);
        return;
      }
    }

    private void ProcessPlayerTurn(int direction, int scale)
    {
      var currentRunningState = _horizontalMove != 0 || _verticalMove != 0;
      if (_currentTurn == direction && _previousRunState == currentRunningState)
        return;
      _currentTurn = direction;
      _previousRunState = currentRunningState;
      TurnTo(direction, currentRunningState);
      SpriteUtil.SetXScale(_body, scale);
    }

    private void TurnTo(int direction, bool isRunning)
    {
      if (!isRunning)
      {
        _animator.SetTrigger(direction);
        return;
      }
      if (direction == PlayerAnimState.Up)
      {
        _animator.SetTrigger(PlayerAnimState.UpRun);
      }
      else if (direction == PlayerAnimState.Down)
      {
        _animator.SetTrigger(PlayerAnimState.DownRun);
      }
      else if (direction == PlayerAnimState.DownRight)
      {
        _animator.SetTrigger(PlayerAnimState.DownRightRun);
      }
      else if (direction == PlayerAnimState.UpRight)
      {
        _animator.SetTrigger(PlayerAnimState.UpRightRun);
      }
    }
  }
}
