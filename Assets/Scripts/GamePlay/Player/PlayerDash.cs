using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GamePlay.Player
{
  public class PlayerDash : MonoBehaviour
  {
    [SerializeField] private GameObject _dashVfx;
    private Rigidbody2D _rigidbody;
    private List<GameObject> _vfxs = new List<GameObject>();
    private bool _canDash = true;
    private float _dashDistance = 3f;
    private CameraFollow _cameraFollow;

    private void Start()
    {
      _rigidbody = GetComponent<Rigidbody2D>();
      _cameraFollow = Camera.main.GetComponent<CameraFollow>();
    }

    private void Update()
    {
      if (_canDash && Input.GetKeyDown(KeyCode.Space))
      {
        DoDash();
      }
    }

    private void DoDash()
    {
      var direction = _rigidbody.velocity.normalized;
      if( direction == Vector2.zero)
        return;
      var end = (Vector2) AppModel.PlayerTransform().position + direction * _dashDistance;

      var wall = Physics2D.LinecastAll(AppModel.PlayerTransform().position, end)
        .FirstOrDefault(x => x.collider.CompareTag("Obstacle"));
      if (wall.collider != null)
        end = wall.point;
      _canDash = false;
      for (var i = 0; i < 4; i++)
      {
        var vfx = Instantiate(_dashVfx);
        vfx.transform.position = Vector2.Lerp(AppModel.PlayerTransform().position , end, i/4f);
        _vfxs.Add(vfx);
      }

      _cameraFollow.Speed = 15;
      AppModel.PlayerTransform().position = end;
      StartCoroutine(EndDash());
    }

    private IEnumerator EndDash()
    {
      yield return new WaitForSeconds(0.1f);
      for (var i = 0; i < 4; i++)
      {
        Destroy(_vfxs[0]);
        _vfxs.RemoveAt(0);
        yield return new WaitForSeconds(0.05f);
      }

      yield return new WaitForSeconds(0.5f);
      _cameraFollow.SetDefaultSpeed();
      _canDash = true;
    }
  }
}