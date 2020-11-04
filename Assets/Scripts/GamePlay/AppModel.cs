using GamePlay.Player;
using UnityEngine;

namespace GamePlay
{
  public static class AppModel
  {
    private static PlayerState _playerState;
    private static Transform _playerTransform;
    private static DropManager _dropManager;

    public static PlayerState Player()
    {
      return _playerState ?? (_playerState = new PlayerState());
    }

    public static Transform PlayerTransform()
    {
      return _playerTransform ? _playerTransform : _playerTransform = GameObject.Find("Player").transform;
    }
        
    public static DropManager Drop()
    {
      return _dropManager ?? (_dropManager = new DropManager());
    }

  }
}