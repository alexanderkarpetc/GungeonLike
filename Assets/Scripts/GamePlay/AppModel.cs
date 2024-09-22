using System.Collections.Generic;
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
    public static ulong OwnerClientId;
    
    private static WeaponStaticData _weaponStaticData;
    
    private static DropManager _dropManager;
    private static ShopManager _shopManager;
    
    private static EnemyFactory _enemyFactory;
    private static SpawnPoint _spawnPointPrefab;
    private static GameObject _bulletContainer;
    private static GameObject _fxContainer;
    private static GameObject _envContainer;
    private static Dictionary<ulong, PlayerState> _playerStates = new();
    private static Dictionary<ulong, GameObject> _playerGameObjs = new();
    private static HudController _hud;

    public static PlayerState PlayerState(ulong? clientId = null)
    {
      clientId ??= OwnerClientId;
      return CollectionExtensions.GetValueOrDefault(_playerStates, clientId.Value);
    }
    
    public static WeaponStaticData WeaponData()
    {
      return _weaponStaticData ??= new WeaponStaticData();
    }

    public static void SetPlayer(PlayerState state, ulong ownerClientId)
    {
      _playerStates[ownerClientId] = state;
      _playerGameObjs[ownerClientId] = state.gameObject;
    }
    
    public static void SetOwner(ulong ownerClientId)
    {
      OwnerClientId = ownerClientId;
    }
    
    public static GameObject PlayerGameObj(ulong? clientId = null)
    {
      clientId ??= OwnerClientId;

      return CollectionExtensions.GetValueOrDefault(_playerGameObjs, clientId.Value);
    }

    public static Transform PlayerTransform(ulong? clientId = null)
    {
      clientId ??= OwnerClientId;

      // Return the player's transform or null if the player GameObject is not found
      return PlayerGameObj(clientId)?.transform;
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
    
    public static GameObject EnvContainer()
    {
      return _envContainer;
    }

    public static void SetContainers(GameObject bullets, GameObject fxs, GameObject env)
    {
      _bulletContainer = bullets;
      _fxContainer = fxs;
      _envContainer = env;
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