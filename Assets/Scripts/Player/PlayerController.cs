using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UIElements;

namespace Player
{
  public class PlayerController : MonoBehaviour
  {
    [SerializeField] private float Speed;
    [SerializeField] private PlayerTurnAnimator playerTurnAnimator;
    [SerializeField] private Rigidbody2D _rigidbody;
    private int _verticalMove;
    private int _horizontalMove;
    private PlayerState _state = new PlayerState();
    private bool _isInvincible;

    private void Update()
    {
      ReadInput();
    }

    public PlayerState GetState()
    {
      return _state;
    }
    private void ReadInput()
    {
      var a = Input.GetKey(KeyCode.A) ? 1 : 0;
      var d = Input.GetKey(KeyCode.D) ? 1 : 0;
      _horizontalMove = d - a;
      playerTurnAnimator.HorizontalMove = _horizontalMove;
      var w = Input.GetKey(KeyCode.W) ? 1 : 0;
      var s = Input.GetKey(KeyCode.S) ? 1 : 0;
      _verticalMove = w - s;
      playerTurnAnimator.VerticalMove = _verticalMove;
      Move();
    }

    private void Move()
    {
      _rigidbody.velocity = new Vector2(_horizontalMove, _verticalMove).normalized * Speed;
    }

    public void Hit()
    {
      if(!_isInvincible)
        StartCoroutine(ApplyHit());
    }

    private IEnumerator ApplyHit()
    {
      _isInvincible = true;
      _state.DealDamage();
      // ScreenBlink
      // Make transparent
      yield return new WaitForSeconds(1f);
      _isInvincible = false;
    }
  }
}