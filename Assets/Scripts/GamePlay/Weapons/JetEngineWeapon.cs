using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GamePlay.Weapons
{
  public class JetEngineWeapon : Weapon
  {
    private enum State
    {
      Idle = 1,
      Charging = 2,
      Shooting = 3
    }

    [SerializeField] private GameObject _middleSegment;

    private float _middleSegmentLength = 27f / 18f;
    private float _chargeTime = 1;
    private State _currentState;

    private float _charged = 0;
    private List<GameObject> _middleSegments = new List<GameObject>();
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Shooting = Animator.StringToHash("Shoot");
    private static readonly int Charging = Animator.StringToHash("Charging");

    protected override void Start()
    {
    }

    private void Update()
    {
      if (_currentState == State.Shooting)
      {
        var direction = IsInverted
          ? DegreeToVector2(transform.rotation.eulerAngles.z) * new Vector2(-1, -1)
          : DegreeToVector2(transform.rotation.eulerAngles.z);
        var hit = Physics2D.RaycastAll(transform.position, direction)
          .First(x => !x.collider.CompareTag("Player") && !x.collider.CompareTag("Projectile"));
        var length = Vector2.Distance(hit.point, _shootPoint.position);
        ClearSegments(length);
        for (var i = _middleSegments.Count; i < length / _middleSegmentLength; i++)
        {
          var instance = Instantiate(_middleSegment, transform);
          instance.transform.localPosition = new Vector3(i * _middleSegmentLength, 0, 0) + _shootPoint.localPosition;
          _middleSegments.Add(instance);
        }
      }
    }

    public void StartCharge()
    {
      _charged = Math.Min(1, _charged + Time.deltaTime);
      if (_charged >= _chargeTime && _currentState != State.Shooting)
      {
        _animator.SetTrigger(Shooting);
        _currentState = State.Shooting;
      }
      else if(_charged < _chargeTime && _currentState != State.Charging)
      {
        _animator.SetTrigger(Charging);
        _currentState = State.Charging;
      }
    }

    public void StopCharge()
    {
      ClearSegments(0);
      if (_charged != 0 && _currentState != State.Idle)
      {
        _animator.SetTrigger(Idle);
        _currentState = State.Idle;
      }

      _charged = 0;
    }

    private void ClearSegments(float length)
    {
      var requiredSegmentsCount = (int)Math.Round(length / _middleSegmentLength, MidpointRounding.ToEven);
      var destroyCount = Math.Max(0, _middleSegments.Count - requiredSegmentsCount);
      for (var i = _middleSegments.Count - destroyCount; i < _middleSegments.Count; i++)
      {
        Destroy(_middleSegments[i]);
      }
      _middleSegments.RemoveRange(_middleSegments.Count-destroyCount, destroyCount);
    }
  }
}