﻿using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Player;
using UnityEngine;

public class TurnAnimator : MonoBehaviour
{
  [SerializeField] private Animator _animator;
  [SerializeField] protected GameObject _body;

  void Update()
  {
    Animate();
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

  protected virtual void ProcessPlayerTurn(int direction, int scale)
  {
    TurnTo(direction, true);
    SpriteUtil.SetXScale(_body, scale);
  }

  protected void TurnTo(int direction, bool isRunning)
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