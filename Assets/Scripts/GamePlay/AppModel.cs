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
    private static GameObject _bulletContainer;
    private static GameObject _fxContainer;
    private static HudController _hud;
    public static StraightLevelController StraightRoomController;


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

    public static GameObject BulletContainer()
    {
      return _bulletContainer;
    }
    
    public static GameObject FxContainer()
    {
      return _fxContainer;
    }

    public static void SetContainers(GameObject bullets, GameObject fxs)
    {
      _bulletContainer = bullets;
      _fxContainer = fxs;
    }

    public static void InitHud(HudController hud)
    {
      _hud = hud;
    }
    
    public static HudController Hud()
    {
      return _hud;
    }
  }
}