using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UIElements;

namespace Player
{
  public class PlayerController : MonoBehaviour
  {
    [SerializeField] private float Speed;
    [SerializeField] private PlayerTurnAnimator _turnAnimator;
    private int _verticalMove;
    private int _horizontalMove;

    private void Update()
    {
      ReadInput();
    }

    private void ReadInput()
    {
      var a = Input.GetKey(KeyCode.A) ? 1 : 0;
      var d = Input.GetKey(KeyCode.D) ? 1 : 0;
      _horizontalMove = d - a;
      _turnAnimator.HorizontalMove = _horizontalMove;
      var w = Input.GetKey(KeyCode.W) ? 1 : 0;
      var s = Input.GetKey(KeyCode.S) ? 1 : 0;
      _verticalMove = w - s;
      _turnAnimator.VerticalMove = _verticalMove;
      Move();
    }

    private void Move()
    {
      transform.Translate(Time.deltaTime * Speed * new Vector2(_horizontalMove, _verticalMove).normalized);
    }
  }
}
