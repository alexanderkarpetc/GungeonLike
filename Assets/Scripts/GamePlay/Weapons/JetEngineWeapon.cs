using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GamePlay.Weapons
{
  public class JetEngineWeapon : Weapon
  {
    private enum ShootingState
    {
      Idle = 1,
      Charging = 2,
      Shooting = 3
    }

    [SerializeField] private GameObject _middleSegment;
    [SerializeField] private GameObject _impactSegment;

    private float _middleSegmentLength = 27f / 18f;
    private float _chargeTime = 1;
    private int _maxSegmentsCount = 10;
    private ShootingState _currentState;
    private int _smallSegmentIndex;

    private float _charged = 0;
    private List<GameObject> _middleSegments = new List<GameObject>();
    private GameObject _impactObj;
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Shooting = Animator.StringToHash("Shoot");
    private static readonly int Charging = Animator.StringToHash("Charging");

    protected override void Start()
    {
    }

    private void Update()
    {
      if (_currentState == ShootingState.Shooting)
      {
        var direction = IsInverted
          ? DegreeToVector2(transform.rotation.eulerAngles.z) * new Vector2(-1, -1)
          : DegreeToVector2(transform.rotation.eulerAngles.z);
        var hit = Physics2D.RaycastAll(transform.position, direction)
          .First(x => !x.collider.CompareTag("Player") && 
                      !x.collider.CompareTag("Projectile") &&
                      !x.collider.CompareTag("Finish"));
        var length = Vector2.Distance(hit.point, _shootPoint.position);
        if (length > _maxSegmentsCount * _middleSegmentLength)
          length = _maxSegmentsCount * _middleSegmentLength;
        var count = (int) Math.Round(length / _middleSegmentLength);
        if (_middleSegments.Count == 0)
        {
          for (var i = 0; i < _maxSegmentsCount; i++)
          {
            var instance = Instantiate(_middleSegment, transform);
            instance.transform.localPosition = new Vector3(i * _middleSegmentLength, 0, 0) + _shootPoint.localPosition;
            _middleSegments.Add(instance);
          }
          var impact = Instantiate(_impactSegment, transform);
          impact.GetComponent<JetEngineImpact>().Weapon = this;
          _impactObj = impact;
        }
        SetVisibleSegments(count - 1, count - length / _middleSegmentLength);
        _impactObj.transform.localPosition = new Vector3(length, 0, 0) + _shootPoint.localPosition;
      }
    }

    public void StartCharge()
    {
      _charged = Math.Min(1, _charged + Time.deltaTime);
      if (_charged >= _chargeTime && _currentState != ShootingState.Shooting)
      {
        _animator.SetTrigger(Shooting);
        _currentState = ShootingState.Shooting;
      }
      else if (_charged < _chargeTime && _currentState != ShootingState.Charging)
      {
        _animator.SetTrigger(Charging);
        _currentState = ShootingState.Charging;
      }
    }

    public void StopCharge()
    {
      ClearSegments();
      Destroy(_impactObj);
      _impactObj = null;
      if (_charged != 0 && _currentState != ShootingState.Idle)
      {
        _animator.SetTrigger(Idle);
        _currentState = ShootingState.Idle;
      }

      _charged = 0;
    }

    private void ClearSegments()
    {
      _middleSegments.ForEach(Destroy);
      _middleSegments.Clear();
    }
    
    private void SetVisibleSegments(int count, float middleSegmentLengthWithdraw)
    {
      if (middleSegmentLengthWithdraw < 0)
        middleSegmentLengthWithdraw = 0;
      _middleSegments[_smallSegmentIndex].transform.localScale = new Vector3(1,1,1);
      for (var i = 0; i < _middleSegments.Count; i++)
      {
        _middleSegments[i].transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = (i <= count);
      }
      if(count<0)
        return;
      _smallSegmentIndex = count;
      _middleSegments[count].transform.localScale = new Vector3(1-middleSegmentLengthWithdraw,1,1);
    }
  }
}