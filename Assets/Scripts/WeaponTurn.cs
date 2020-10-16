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
    
    protected float Angle;

    private void Start()
    {
      _rightHandSprite = _rightHand.GetComponent<SpriteRenderer>();
      _leftHandSprite = _leftHand.GetComponent<SpriteRenderer>();
      _leftHand.localPosition = _leftHandPos;
      _rightHand.localPosition = _rightHandPos;
      OnStart();
    }

    protected virtual void OnStart() { }

    void Update()
    {
      TurnGun();
      ChangeSortingOrder();
      MoveHands();
    }

    protected virtual void TurnGun() { }

    protected virtual void MoveHands() { }

    protected virtual void ChangeSortingOrder() { }
  }
}