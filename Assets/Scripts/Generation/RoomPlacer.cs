using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DefaultNamespace;
using GamePlay.Level;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;
using Random = UnityEngine.Random;

public class RoomPlacer : MonoBehaviour
{
  [SerializeField] GameObject[] Room1Door;
  [SerializeField] GameObject[] Room2Door;
  [SerializeField] GameObject[] Room3Door;
  [SerializeField] GameObject[] Room4Door;
  [SerializeField] GameObject[] BossRooms;
  [SerializeField] private GameObject _astar;

  private List<GameObject> RoomObj;
  public Vector2 roomDimensions = new Vector2(1, 1);
  public Vector2 gutterSize = new Vector2(1, 1);

  private RoomSetup[,] _rooms;
  private Dictionary<RoomSetup, GameObject> roomObjects = new Dictionary<RoomSetup, GameObject>();
  private static GameObject _parentObj;

  private void Start()
  {
    RoomObj = Room1Door.Concat(Room2Door).Concat(Room3Door).Concat(Room4Door).ToList();
  }

  public void Place(RoomSetup[,] rooms)
  {
    _rooms = rooms;
    Preprocess();
    DoPlace();
    PostProcess();
  }

  private void Preprocess()
  {
  }

  private void DoPlace()
  {
    _parentObj = Util.InitParentIfNeed("Level");
    foreach (var room in _rooms)
    {
      if (room == null)
      {
        continue;
      }

      var pos = new Vector3(room.GridPos.x * (roomDimensions.x + gutterSize.x),
        room.GridPos.y * (roomDimensions.y + gutterSize.y), 0);
      var roomObj = FindSuitableRoom(room);
      var go = Instantiate(roomObj, pos, Quaternion.identity, _parentObj.transform);
      roomObjects.Add(room, go);
    }
  }

  private GameObject FindSuitableRoom(RoomSetup roomSetup)
  {
    List<GameObject> allRooms;
    switch (roomSetup.Kind)
    {
      case RoomSetup.RoomKind.Start:
        allRooms = RoomObj.ToList();
        break;
      case RoomSetup.RoomKind.Normal:
        allRooms = RoomObj.ToList();
        break;
      case RoomSetup.RoomKind.Boss:
        allRooms = BossRooms.ToList();
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }

    var sb = new StringBuilder();
    sb.Append(roomSetup.doorDown ? "d" : "");
    sb.Append(roomSetup.doorUp ? "u" : "");
    sb.Append(roomSetup.doorLeft ? "l" : "");
    sb.Append(roomSetup.doorRight ? "r" : "");
    var roomDoors = sb.ToString().ToCharArray();
    var suitableRooms = allRooms.Where(obj =>
    {
      var chars = obj.name.ToLower().Where(char.IsLetter).ToList();
      return chars.All(roomDoors.Contains) && chars.Count == roomDoors.Length;
    }).ToList();

    var index = Random.Range(0, suitableRooms.Count - 1);
    return suitableRooms[index];
  }
  
  private void PostProcess()
  {
    foreach (var room in roomObjects)
    {
      var roomController = room.Value.AddComponent<RoomController>();
      roomController.setup = room.Key;
      roomController.State = new RoomState();
      SetCollider(room);
    }

    InitAstar();
  }

  private static void SetCollider(KeyValuePair<RoomSetup, GameObject> room)
  {
    var boxCollider = room.Value.AddComponent<BoxCollider2D>();
    boxCollider.isTrigger = true;
    var roofCollider = room.Value.transform.Find("Roof").GetComponent<Collider2D>();
    boxCollider.offset = roofCollider.offset;
    boxCollider.size = roofCollider.bounds.extents * 2 - new Vector3(1, 0);
    room.Value.tag = "Environment";
  }
      
  private void InitAstar()
  {
    Instantiate(_astar);
  }
}