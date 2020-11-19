using GamePlay.Level;
using GamePlay.Player;
using UnityEngine;

namespace GamePlay
{
  public static class AppModel
  {
    static AppModel()
    {
      random.InitState();
    }

    public static Unity.Mathematics.Random random = new Unity.Mathematics.Random();

    private static PlayerState _playerState;
    private static Transform _playerTransform;
    private static DropManager _dropManager;
    private static EnemyFactory _enemyFactory;
    private static GameObject _playerGameObj;
    private static SpawnPoint _spawnPointPrefab;


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
    
    public static EnemyFactory EnemyFactory()
    {
      return _enemyFactory ?? (_enemyFactory = new EnemyFactory());
    }  
    
    public static SpawnPoint SpawnPointPrefab()
    {
      return _spawnPointPrefab ? _spawnPointPrefab : _spawnPointPrefab = Resources.Load<SpawnPoint>("Prefabs/Env/Secondary/SpawnPoint");
    }

  }
}