using System;
using DefaultNamespace;
using UnityEngine;

namespace Player
{
  public class WeaponTurn : MonoBehaviour
  {
    [SerializeField] private SpriteRenderer _body;
    [SerializeField] private Transform _secondaryHandPos;
    [SerializeField] private Vector3 _leftHandPos;
    [SerializeField] private Vector3 _rightHandPos;
    [SerializeField] private Weapon Weapon;

    private Transform _rightHand;
    private SpriteRenderer _rightHandSprite;
    private Transform _leftHand;
    private SpriteRenderer _leftHandSprite;
    private Animator _playerAnimator;
    private float _angle;

    private void Start()
    {
      _rightHand = GameObject.Find("RightHand").transform;
      _leftHand = GameObject.Find("LeftHand").transform;
      _rightHandSprite = _rightHand.GetComponent<SpriteRenderer>();
      _leftHandSprite = _leftHand.GetComponent<SpriteRenderer>();
      _playerAnimator = GameObject.Find("Player").GetComponent<Animator>();
      _leftHand.position = _leftHandPos;
      _rightHand.position = _rightHandPos;
    }

    void Update()
    {
      TurnGun();
      MoveHands();
    }

    private void TurnGun()
    {
      Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
      _angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
      var rot = Quaternion.AngleAxis(Mathf.Abs(_angle) < 90 ? _angle : - 180 + _angle, Vector3.forward);
      transform.rotation = rot;
    }

    private void MoveHands()
    {
      var state = _playerAnimator.GetCurrentAnimatorStateInfo(0);
      if (state.IsName(PlayerAnimState.IdleDown) || state.IsName(PlayerAnimState.RunDown) ||
          state.IsName(PlayerAnimState.IdleDownRight) || state.IsName(PlayerAnimState.RunDownRight))
      {
        _body.sortingOrder = 1;
        _rightHandSprite.sortingOrder = 2;
        _leftHandSprite.sortingOrder = 2;
      }
      else if (state.IsName(PlayerAnimState.IdleUp) || state.IsName(PlayerAnimState.RunUp) ||
               state.IsName(PlayerAnimState.IdleUpRight) || state.IsName(PlayerAnimState.RunUpRight))
      {
        _body.sortingOrder = -2;
        _rightHandSprite.sortingOrder = -2;
        _leftHandSprite.sortingOrder = -2;
      }

      if (Mathf.Abs(_angle) < 90)
      {
        _leftHand.localPosition = _leftHandPos;
        _rightHand.position = _secondaryHandPos.position;
        gameObject.transform.position = _leftHand.position;
        SpriteUtil.SetXScale(gameObject, 1);
        Weapon.IsInverted = false;
      }
      else
      {
        _rightHand.localPosition = _rightHandPos;
        _leftHand.position = _secondaryHandPos.position;
        gameObject.transform.position = _rightHand.position;
        SpriteUtil.SetXScale(gameObject, -1);
        Weapon.IsInverted = true;
      }
    }
  }
}