using System;
using System.Collections;
using GamePlay.Common;
using GamePlay.Weapons;
using UnityEngine;

namespace GamePlay.Player
{
  public class PlayerController : MonoBehaviour
  {
    [SerializeField] private PlayerTurnAnimator playerTurnAnimator;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private SpriteRenderer _body;
    [SerializeField] private Weapon _startingWeapon;
    private int _verticalMove;
    private int _horizontalMove;
    private bool _isInvincible;
    private PlayerInitializer _initializer = new PlayerInitializer();
    
    public void Init()
    {
      _initializer.Init(_startingWeapon);
    }

    private void Update()
    {
      ReadInput();
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
      _rigidbody.velocity = new Vector2(_horizontalMove, _verticalMove).normalized * StaticData.PlayerSpeedBase * AppModel.Player().SpeedMultiplier;
    }

    public void Hit()
    {
      if(!_isInvincible)
        StartCoroutine(ApplyHit());
    }

    private IEnumerator ApplyHit()
    {
      _isInvincible = true;
      var state = AppModel.Player();
      state.DealDamage();
      if (state.GetHp() <= 0)
        Die();
      // ScreenBlink
      var bodyColor = _body.color;
      bodyColor.a = 0.5f;
      _body.color = bodyColor;
      yield return new WaitForSeconds(1f);
      bodyColor.a = 1;
      _body.color = bodyColor;
      _isInvincible = false;
    }

    private void Die()
    {
      Destroy(gameObject);
    }
  }
}