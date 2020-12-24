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
    private int _maxSegmentsCount = 10;
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
        if (_middleSegments.Count == 0)
        {
          for (var i = 0; i < _maxSegmentsCount; i++)
          {
            var instance = Instantiate(_middleSegment, transform);
            instance.transform.localPosition = new Vector3(i * _middleSegmentLength, 0, 0) + _shootPoint.localPosition;
            _middleSegments.Add(instance);
          }
        }

        var direction = IsInverted
          ? DegreeToVector2(transform.rotation.eulerAngles.z) * new Vector2(-1, -1)
          : DegreeToVector2(transform.rotation.eulerAngles.z);
        var hit = Physics2D.RaycastAll(transform.position, direction)
          .First(x => !x.collider.CompareTag("Player") && !x.collider.CompareTag("Projectile"));
        var length = Vector2.Distance(hit.point, _shootPoint.position);
        var count = (int) Math.Round(length / _middleSegmentLength);
        SetVisibleSegments(count);
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
      else if (_charged < _chargeTime && _currentState != State.Charging)
      {
        _animator.SetTrigger(Charging);
        _currentState = State.Charging;
      }
    }

    public void StopCharge()
    {
      ClearSegments();
      if (_charged != 0 && _currentState != State.Idle)
      {
        _animator.SetTrigger(Idle);
        _currentState = State.Idle;
      }

      _charged = 0;
    }

    private void ClearSegments()
    {
      _middleSegments.ForEach(Destroy);
      _middleSegments.Clear();
    }
    
    private void SetVisibleSegments(int count)
    {
      for (var i = 0; i < _middleSegments.Count; i++)
      {
        _middleSegments[i].transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = (i < count);
      }
    }
  }
}