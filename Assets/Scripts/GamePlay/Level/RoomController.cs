using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using GamePlay.Enemy;
using Pathfinding.Examples;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace GamePlay.Level
{
  public class RoomController : MonoBehaviour
  {
    public RoomSetup setup;
    public RoomState State;
    public GameObject Fog;
    private Vector3 _posUp;
    private Vector3 _posDown;
    private Vector3 _posLeft;
    private Vector3 _posRight;
    private Bounds _bounds;
    
    private List<GameObject> _doors = new List<GameObject>();
    private List<EnemyController> _enemies = new List<EnemyController>();
    private static readonly int GoUp = Animator.StringToHash("GoUp");

    private void Start()
    {
      _bounds = transform.Find("Roof").GetComponent<Collider2D>().bounds;
      _posUp =  new Vector2(_bounds.center.x, _bounds.max.y) + new Vector2(-0.5f, 1);
      _posDown =  new Vector2(_bounds.center.x, _bounds.min.y) + new Vector2(-0.5f, -1.5f);
      _posLeft =  new Vector2(_bounds.min.x, _bounds.center.y) + new Vector2(-0.5f, 1.5f);
      _posRight =  new Vector2(_bounds.max.x, _bounds.center.y) + new Vector2(0, 1.5f);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
      if(!other.CompareTag("Player"))
        return;
      Destroy(gameObject.GetComponent<Collider2D>());
      if (!State.IsCleaned && !State.IsVisited && setup.Kind != RoomSetup.RoomKind.Start)
      {
        CloseDoors();
        SpawnEnemies();
        State.IsVisited = true;
      } 
    }

    private void CloseDoors()
    {
      if (setup.doorUp)
      {
        var doorUp = Resources.Load<GameObject>("Prefabs/Env/Doors/DoorFront");
        var go = Instantiate(doorUp, _posUp, Quaternion.identity, gameObject.transform);
        go.transform.Find("Body").GetComponent<SpriteRenderer>().sortingOrder = 5;
        _doors.Add(go);
      }
      if (setup.doorDown)
      {
        var doorDown = Resources.Load<GameObject>("Prefabs/Env/Doors/DoorFront");
        var go = Instantiate(doorDown, _posDown, Quaternion.identity, gameObject.transform);
        go.transform.Find("Body").GetComponent<SpriteRenderer>().sortingOrder = 10;
        _doors.Add(go);
      }
      if (setup.doorLeft)
      {
        var doorLeft = Resources.Load<GameObject>("Prefabs/Env/Doors/DoorSide");
        var go = Instantiate(doorLeft, _posLeft, Quaternion.identity, gameObject.transform);
        _doors.Add(go);
      }
      if (setup.doorRight)
      {
        var doorRight = Resources.Load<GameObject>("Prefabs/Env/Doors/DoorSide");
        var go = Instantiate(doorRight, _posRight, Quaternion.identity, gameObject.transform);
        _doors.Add(go);
      }
    }
    
    private void SpawnEnemies()
    {
      var quantity = AppModel.random.NextInt(3, 5);
      for (var i = 0; i < quantity; i++)
      {
        var randomEnemy = AppModel.EnemyFactory().GetRandomEnemy();
        SpawnEnemy(randomEnemy);
      }
    }

    private void SpawnEnemy(EnemySetup enemySetup)
    {
      var xCoord = AppModel.random.NextFloat(_bounds.min.x, _bounds.max.x);
      var yCoord = AppModel.random.NextFloat(_bounds.min.y, _bounds.max.y);
      var spawnPoint = Instantiate(AppModel.SpawnPointPrefab(), new Vector3(xCoord, yCoord, 0), Quaternion.identity);
      spawnPoint.OnSpawn += OnSpawn;
      spawnPoint.SpawnObject = enemySetup.EnemyObject;
    }

    private void OnSpawn(EnemyController enemyController)
    {
      enemyController.OnDeath += OnEnemyDeath;
      _enemies.Add(enemyController);
    }

    private void OnEnemyDeath(EnemyController enemyController)
    {
      _enemies.Remove(enemyController);
      if (_enemies.Count == 0)
        OpenDoors();
    }

    private void OpenDoors()
    {
      StartCoroutine(DoOpen());
    }

    private IEnumerator DoOpen()
    {
      _doors.ForEach(x =>
      {
        var animator = x.transform.GetComponent<Animator>();
        animator.SetTrigger(GoUp);
      });
      yield return new WaitForSeconds(1);
      _doors.ForEach(Destroy);
    }
  }
}