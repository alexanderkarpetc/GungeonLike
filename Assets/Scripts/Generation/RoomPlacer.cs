using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;

public class RoomPlacer : MonoBehaviour
{
  [SerializeField] GameObject[] RoomObj;
  [SerializeField] TileBase floor;

  public Vector2 roomDimensions = new Vector2(1, 1);
  public Vector2 gutterSize = new Vector2(1, 1);
  
  private Room[,] _rooms;
  private Dictionary<Room, GameObject> roomObjects = new Dictionary<Room, GameObject>();
  private static GameObject _parentObj;

  public void Place(Room[,] rooms)
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

      var pos = new Vector3(room.gridPos.x * (roomDimensions.x + gutterSize.x),
        room.gridPos.y * (roomDimensions.y + gutterSize.y), 0);
      var roomObj = FindSuitableRoom(room);
      var go = Instantiate(roomObj, pos, Quaternion.identity, _parentObj.transform);
      roomObjects.Add(room, go);
    }
  }

  private GameObject FindSuitableRoom(Room room)
  {
    var allRooms = RoomObj.ToList();
    var suitableRooms = allRooms.Where(obj =>
    {
      var chars = obj.name.ToLower().Where(char.IsLetter).ToList();
      if (room.doorDown)
      {
        if (!CheckRoomHasLetter(chars, 'd'))
          return false;
      }
      if (room.doorUp)
      {
        if (!CheckRoomHasLetter(chars, 'u'))
          return false;
      }
      if (room.doorLeft)
      {
        if (!CheckRoomHasLetter(chars, 'l'))
          return false;
      }
      if (room.doorRight)
      {
        if (!CheckRoomHasLetter(chars, 'r'))
          return false;
      }

      if (chars.Count == 0)
        return true;
      else
        return false;
    }).ToList();

    var index = Random.Range(0, suitableRooms.Count -1);
    return suitableRooms[index];
    bool CheckRoomHasLetter(List<char> chars, char letter)
    {
      if (!chars.Contains(letter))
        return false;
      chars.Remove(letter);
      return true;
    }
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