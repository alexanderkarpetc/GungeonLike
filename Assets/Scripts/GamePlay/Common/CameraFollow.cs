using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GamePlay
{
  public class CameraFollow : MonoBehaviour
  {
    private Transform _player;
    private static float _defaultSpeed = 200f;
    public float Speed;

    private void Start()
    {
      InitCamera().Forget();
    }

    private async UniTask InitCamera()
    {
      await UniTask.WaitUntil(() => AppModel.PlayerTransform() != null);
      _player = AppModel.PlayerTransform();
      SetDefaultSpeed();
    }

    public void SetDefaultSpeed()
    {
      Speed = _defaultSpeed;
    }

    private void Update()
    {
      if (_player != null)
      {
        var targetPos = new Vector3(_player.position.x, _player.position.y, -10);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, Speed * Time.deltaTime);
      }
    }
  }
}