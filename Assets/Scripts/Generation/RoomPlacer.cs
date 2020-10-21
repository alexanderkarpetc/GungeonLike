using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

  private List<GameObject> RoomObj;
  public Vector2 roomDimensions = new Vector2(1, 1);
  public Vector2 gutterSize = new Vector2(1, 1);

  private RoomState[,] _rooms;
  private Dictionary<RoomState, GameObject> roomObjects = new Dictionary<RoomState, GameObject>();
  private static GameObject _parentObj;

  private void Start()
  {
    RoomObj = Room1Door.Concat(Room2Door).Concat(Room3Door).Concat(Room4Door).ToList();
  }

  public void Place(RoomState[,] rooms)
  {
    _rooms = rooms;
    Preprocess();
    DoPlace();
    // PostProcess();
  }

  private void Preprocess()
  {
  }

  private void DoPlace()
  {
    InitParentIfNeed();
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

  private GameObject FindSuitableRoom(RoomState roomState)
  {
    List<GameObject> allRooms;
    switch (roomState.Kind)
    {
      case RoomState.RoomKind.Start:
        allRooms = RoomObj.ToList();
        break;
      case RoomState.RoomKind.Normal:
        allRooms = RoomObj.ToList();
        break;
      case RoomState.RoomKind.Boss:
        allRooms = BossRooms.ToList();
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }

    var sb = new StringBuilder();
    sb.Append(roomState.doorDown ? "d" : "");
    sb.Append(roomState.doorUp ? "u" : "");
    sb.Append(roomState.doorLeft ? "l" : "");
    sb.Append(roomState.doorRight ? "r" : "");
    var roomDoors = sb.ToString().ToCharArray();
    var suitableRooms = allRooms.Where(obj =>
    {
      var chars = obj.name.ToLower().Where(char.IsLetter).ToList();
      return chars.All(roomDoors.Contains) && chars.Count == roomDoors.Length;
    }).ToList();

    var index = Random.Range(0, suitableRooms.Count - 1);
    return suitableRooms[index];
  }


  private static void InitParentIfNeed()
  {
    var levelParent = GameObject.Find("Level");
    if (levelParent == null)
    {
      levelParent = new GameObject("Level");
    }

    _parentObj = levelParent;
  }
}