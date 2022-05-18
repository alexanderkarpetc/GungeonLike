using UnityEngine;

namespace GamePlay
{
  public class CameraFollow : MonoBehaviour
  {
    private Transform _player;
    public float Speed;

    void Start()
    {
      _player = AppModel.PlayerTransform();
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