using GamePlay.Weapons;
using UnityEngine;

namespace GamePlay
{
  public class WeaponTurn : MonoBehaviour
  {
    [SerializeField] protected Vector3 _leftHandPos;
    [SerializeField] protected Vector3 _rightHandPos;
    [SerializeField] public Weapon Weapon;

    public Transform _rightHand;
    public Transform _leftHand;
    protected SpriteRenderer _leftHandRenderer;
    protected SpriteRenderer _rightHandRenderer;
    
    protected float Angle;

    private void Start()
    {
      _leftHand.localPosition = _leftHandPos;
      _rightHand.localPosition = _rightHandPos;
      _leftHandRenderer = _leftHand.GetComponent<SpriteRenderer>();
      _rightHandRenderer = _rightHand.GetComponent<SpriteRenderer>();
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

    private void MoveHands()
    {
      if (Mathf.Abs(Angle) < 90)
      {
        _leftHand.localPosition = _leftHandPos;
        gameObject.transform.position = !Weapon.IsDoubleHanded ? _rightHand.position : _leftHand.position;
        SpriteUtil.SetXScale(gameObject, 1);
        Weapon.IsInverted = false;
        if(Weapon.SecondHandPos != null)
          _rightHand.position = Weapon.SecondHandPos.position;
      }
      else
      {
        _rightHand.localPosition = _rightHandPos;
        gameObject.transform.position = !Weapon.IsDoubleHanded ? _leftHand.position : _rightHand.position;
        SpriteUtil.SetXScale(gameObject, -1);
        Weapon.IsInverted = true;
        if(Weapon.SecondHandPos != null)
          _leftHand.position = Weapon.SecondHandPos.position;
      }
    }

    protected virtual void ChangeSortingOrder() { }
  }
}