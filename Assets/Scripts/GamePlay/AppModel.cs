using GamePlay.Player;
using UnityEngine;

namespace GamePlay
{
  public static class AppModel
  {
    private static PlayerState _playerState;
    private static Transform _playerTransform;
    private static DropManager _dropManager;
    private static GameObject _playerGameObj;

    public static PlayerState Player()
    {
      return _playerState ?? (_playerState = new PlayerState());
    }

    public static void SetPlayer(GameObject player)
    {
      _playerGameObj = player;
    }
    
    public static GameObject PlayerGameObj()
    {
      return _playerGameObj;
    }

    public static Transform PlayerTransform()
    {
      return PlayerGameObj().transform;
    }
        
    public static DropManager Drop()
    {
      return _dropManager ?? (_dropManager = new DropManager());
    }

  }
}