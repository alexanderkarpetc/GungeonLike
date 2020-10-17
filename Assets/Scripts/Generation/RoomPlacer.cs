﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;

public class RoomPlacer : MonoBehaviour
{
  [SerializeField] GameObject[] RoomObj;
  [SerializeField] GameObject CorridorUpDown;
  [SerializeField] GameObject CorridorLeftRight;
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
    PostProcess();
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
      var index = Random.Range(0, RoomObj.Length - 1);
      var go = Instantiate(RoomObj[index], pos, Quaternion.identity, _parentObj.transform);
      roomObjects.Add(room, go);
    }
  }

  private void PostProcess()
  {
    foreach (var roomObject in roomObjects)
    {
      var tilemapMain = roomObject.Value.transform.Find("Main").GetComponent<Tilemap>();
      var tilemapRoof = roomObject.Value.transform.Find("Roof").GetComponent<Tilemap>();
      var tilemapWall = roomObject.Value.transform.Find("Wall").GetComponent<Tilemap>();
      var cellBounds = tilemapMain.cellBounds;
      var center = cellBounds.min + cellBounds.max;
      var size = cellBounds.size;
      if (roomObject.Key.doorBot)
      {
        SetFloor(tilemapRoof,   tilemapWall, tilemapMain, center.x - 1 ,(center.y - size.y) / 2);
        SetFloor(tilemapRoof,  tilemapWall, tilemapMain, center.x - 2 ,(center.y - size.y) / 2);
      }
      if (roomObject.Key.doorTop)
      {
        SetFloor(tilemapRoof,   tilemapWall, tilemapMain, center.x-1 ,(center.y + size.y) / 2 - 1);
        SetFloor(tilemapRoof,  tilemapWall, tilemapMain, center.x - 2 ,(center.y + size.y) / 2 - 1);
        SetFloor(tilemapRoof,   tilemapWall, tilemapMain, center.x-1 ,(center.y + size.y) / 2 - 2);
        SetFloor(tilemapRoof,  tilemapWall, tilemapMain, center.x - 2 ,(center.y + size.y) / 2 - 2);
        var offset = new Vector3(center.x-1, center.y+ size.y/2 -2,0);
        Instantiate(CorridorUpDown, roomObject.Value.transform.position + offset, Quaternion.identity, roomObject.Value.transform);
      }
      if (roomObject.Key.doorRight)
      {
        SetFloor(tilemapRoof,  tilemapWall, tilemapMain, -center.x+ size.x/2 ,-1);
        SetFloor(tilemapRoof,   tilemapWall, tilemapMain, -center.x+ size.x/2 ,0);
        SetFloor(tilemapRoof,   tilemapWall, tilemapMain, -center.x+ size.x/2 ,1);
        
        var offset = new Vector3(-center.x+ size.x/2, 0,0);
        Instantiate(CorridorLeftRight, roomObject.Value.transform.position + offset, Quaternion.identity, roomObject.Value.transform);
      }
      if (roomObject.Key.doorLeft)
      {
        SetFloor(tilemapRoof,   tilemapWall, tilemapMain, center.x - size.x / 2 + 2 ,1);
        SetFloor(tilemapRoof,   tilemapWall, tilemapMain, center.x - size.x / 2 + 2 ,0);
        SetFloor(tilemapRoof,  tilemapWall, tilemapMain, center.x - size.x / 2 + 2,-1);
      }
    }
  }

  private void SetFloor(Tilemap tilemapRoof, Tilemap tilemapWall, Tilemap tilemapMain, int x, int y)
  {
    tilemapRoof.SetTile(new Vector3Int(x, y, 0), null);
    tilemapWall.SetTile(new Vector3Int(x, y, 0), null);
    // tilemapMain.SetTile(new Vector3Int(x, y, 0), floor);
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