using System;
using DefaultNamespace;
using UnityEngine;

namespace Player
{
  public class WeaponTurn : MonoBehaviour
  {
    [SerializeField] protected SpriteRenderer _body;
    [SerializeField] protected Transform _secondaryHandPos;
    [SerializeField] protected Vector3 _leftHandPos;
    [SerializeField] protected Vector3 _rightHandPos;
    [SerializeField] protected Weapon Weapon;

    public Transform _rightHand;
    public Transform _leftHand;

    protected SpriteRenderer _rightHandSprite;
    protected SpriteRenderer _leftHandSprite;
    
    protected Animator _playerAnimator;
    protected float Angle;

    private void Start()
    {
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

    protected virtual void TurnGun()
    {
      Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
      Angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
      var rot = Quaternion.AngleAxis(Mathf.Abs(Angle) < 90 ? Angle : - 180 + Angle, Vector3.forward);
      transform.rotation = rot;
    }

    private void MoveHands()
    {
      var state = _playerAnimator.GetCurrentAnimatorStateInfo(0);
      ChangeSortingOrder(state);

      if (Mathf.Abs(Angle) < 90)
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

    protected virtual void ChangeSortingOrder(AnimatorStateInfo state) { }
  }
}