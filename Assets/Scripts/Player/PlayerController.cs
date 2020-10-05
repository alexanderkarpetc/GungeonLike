using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Player
{
  public class PlayerController : MonoBehaviour
  {
    [SerializeField] private Animator _animator;
    [SerializeField] private float Speed;
    private int _verticalMove;
    private int _horizontalMove;
    private int _currentTurn;
    private static readonly int DownRight = Animator.StringToHash("downRight");
    private static readonly int UpRight = Animator.StringToHash("upRight");
    private static readonly int Up = Animator.StringToHash("up");
    private static readonly int Down = Animator.StringToHash("down");
    private static readonly int UpRun = Animator.StringToHash("upRun");
    private static readonly int DownRun = Animator.StringToHash("downRun");
    private static readonly int DownRightRun = Animator.StringToHash("downRightRun");
    private static readonly int UpRightRun = Animator.StringToHash("upRightRun");
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
        ProcessPlayerTurn(DownRight, 1);
        return;
      }
      if (angle >= 25 && angle < 70)
      {
        ProcessPlayerTurn(UpRight, 1);
        return;
      }
      if (angle >= 70 && angle < 110)
      {
        ProcessPlayerTurn(Up, 1);
        return;
      }
      if (angle >= 110 && angle < 155)
      {
        ProcessPlayerTurn(UpRight, -1);
        return;
      }
      
      if (angle > -135 && angle < -75)
      {
        ProcessPlayerTurn(Down, 1);
        return;
      }
      
      if (Mathf.Abs(angle) >= 135)
      {
        ProcessPlayerTurn(DownRight, -1);
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
      SetXScale(scale);
    }

    private void TurnTo(int direction, bool isRunning)
    {
      if (!isRunning)
      {
        _animator.SetTrigger(direction);
        return;
      }
      if (direction == Up)
      {
        _animator.SetTrigger(UpRun);
      }
      else if (direction == Down)
      {
        _animator.SetTrigger(DownRun);
      }
      else if (direction == DownRight)
      {
        _animator.SetTrigger(DownRightRun);
      }
      else if (direction == UpRight)
      {
        _animator.SetTrigger(UpRightRun);
      }
    }

    private void SetXScale(int x)
    {
      var transform1 = transform;
      var newScale = transform1.localScale;
      newScale.x = x;
      transform1.localScale = newScale;
    }
  }
}
