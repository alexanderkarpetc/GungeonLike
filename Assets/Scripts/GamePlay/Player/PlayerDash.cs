using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GamePlay.Player
{
  public class PlayerDash : MonoBehaviour
  {
    [SerializeField] private GameObject _dashVfx;
    private List<GameObject> _vfxs = new List<GameObject>();
    private bool _canDash = true;

    private void Update()
    {
      if (_canDash && Input.GetKeyDown(KeyCode.Space))
      {
        DoDash();
      }
    }

    private void DoDash()
    {
      _canDash = false;
      Vector2 direction = ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position).normalized / 1.5f;
      for (var i = 0; i < 4; i++)
      {
        var vfx = Instantiate(_dashVfx);
        vfx.transform.position = (Vector2)AppModel.PlayerTransform().position + direction * i;
        _vfxs.Add(vfx);
      }

      AppModel.PlayerTransform().position = _vfxs.Last().transform.position;
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
      _canDash = true;
    }
  }
}