using GamePlay.Common;
using GamePlay.Level;
using GamePlay.Level.Controllers;
using GamePlay.Player;
using UnityEngine;

namespace GamePlay
{
  public static class AppModel
  {
    static AppModel()
    {
      random.InitState((uint)System.DateTime.Now.Millisecond);
    }

    public static Unity.Mathematics.Random random = new Unity.Mathematics.Random();

    public static StraightLevelController StraightRoomController;
    public static StraightRoomController CurrentRoom;
    
    private static WeaponStaticData _weaponStaticData;
    
    private static PlayerState _playerState;
    private static DropManager _dropManager;
    private static ShopManager _shopManager;
    
    private static Transform _playerTransform;
    private static EnemyFactory _enemyFactory;
    private static SpawnPoint _spawnPointPrefab;
    private static GameObject _bulletContainer;
    private static GameObject _fxContainer;
    private static GameObject _playerGameObj;
    private static HudController _hud;



    public static PlayerState Player()
    {
      return _playerState ?? (_playerState = new PlayerState());
    }
    
    public static WeaponStaticData WeaponData()
    {
      return _weaponStaticData ??= new WeaponStaticData();
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
        
    public static DropManager DropManager()
    {
      return _dropManager ?? (_dropManager = new DropManager());
    }
    
    public static ShopManager ItemManager()
    {
      return _shopManager ?? (_shopManager = new ShopManager());
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