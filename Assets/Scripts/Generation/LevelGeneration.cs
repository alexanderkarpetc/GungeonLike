using System;

namespace DefaultNamespace
{
  using System.Collections;
  using System.Collections.Generic;
  using UnityEngine;

  public class LevelGeneration : MonoBehaviour
  {
    [SerializeField] private int _numberOfRooms = 20;
    [SerializeField] private Vector2 roomsPlacementOffset = new Vector2(4, 4);
    [SerializeField] private RoomPlacer _roomPlacer;
    public Transform mapRoot;

    MazeRoomSetup[,] rooms;
    private readonly List<Vector2> _takenPositions = new List<Vector2>();
    private int _gridSizeX;
    private int _gridSizeY;

    const float RandomCompareStart = 0.2f;
    const float RandomCompareEnd = 0.01f;

    void Start()
    {
      if (_numberOfRooms >= (roomsPlacementOffset.x * 2) * (roomsPlacementOffset.y * 2))
      {
        throw new ArithmeticException("Ti eblan komnat slishkom mnogo");
      }

      _gridSizeX = Mathf.RoundToInt(roomsPlacementOffset.x); //note: these are half-extents
      _gridSizeY = Mathf.RoundToInt(roomsPlacementOffset.y);
      CreateLevel(); // tmp
    }

    public void CreateLevel()
    {
      CreateRooms();
      SetRoomDoors();
      DefineBossRoom();
      _roomPlacer.Place(rooms);
    }

    private void DefineBossRoom()
    {
      var suitableRooms = new List<MazeRoomSetup>();
      foreach (var room in rooms)
      {
        if(room != null && NumberOfNeighbors(room) == 1)
          suitableRooms.Add(room);
      }

      suitableRooms[Random.Range(0, suitableRooms.Count - 1)].Kind = MazeRoomSetup.RoomKind.Boss;
    }
    private void CreateRooms()
    {
      //setup
      rooms = new MazeRoomSetup[_gridSizeX * 2, _gridSizeY * 2];
      rooms[_gridSizeX, _gridSizeY] = new MazeRoomSetup(Vector2.zero, MazeRoomSetup.RoomKind.Start);
      _takenPositions.Add(Vector2.zero);
      Vector2 checkPos;

      for (var i = 0; i < _numberOfRooms - 1; i++)
      {
        var randomPerc = i / ((float) _numberOfRooms - 1);
        var randomCompare = Mathf.Lerp(RandomCompareStart, RandomCompareEnd, randomPerc);
        checkPos = NewPosition();
        //test new position
        if (NumberOfNeighbors(checkPos) > 1 && Random.value > randomCompare)
        {
          var iterations = 0;

          while (NumberOfNeighbors(checkPos) > 1)
          {
            checkPos = SelectiveNewPosition();
          }

          if (iterations++ >= 50)
            throw new ArithmeticException($"Wasn't able to create room pos within {iterations} iterations");
        }

        //finalize position
        rooms[(int) checkPos.x + _gridSizeX, (int) checkPos.y + _gridSizeY] = new MazeRoomSetup(checkPos, MazeRoomSetup.RoomKind.Normal);
        _takenPositions.Add(checkPos);
      }
    }

    private Vector2 NewPosition()
    {
      var x = 0;
      var y = 0;
      var checkingPos = Vector2.zero;
      while (_takenPositions.Contains(checkingPos) || x >= _gridSizeX || x < -_gridSizeX || y >= _gridSizeY ||
             y < -_gridSizeY)
      {
        var index = Mathf.RoundToInt(Random.value * (_takenPositions.Count - 1)); // pick a random room
        x = (int) _takenPositions[index].x;
        y = (int) _takenPositions[index].y;
        var up = Random.value < 0.5f;
        var positive = Random.value < 0.5f;
        if (up)
        {
          y += positive ? 1 : -1;
        }
        else
        {
          x += positive ? 1 : -1;
        }

        checkingPos = new Vector2(x, y);
      }

      return checkingPos;
    }

    private int NumberOfNeighbors(MazeRoomSetup mazeRoomSetup)
    {
      return (mazeRoomSetup.doorDown ? 1 : 0) + (mazeRoomSetup.doorUp ? 1 : 0) + (mazeRoomSetup.doorLeft ? 1 : 0) + (mazeRoomSetup.doorRight ? 1 : 0);
    }
    private int NumberOfNeighbors(Vector2 checkingPos)
    {
      var neighbors = 0; // start at zero, add 1 for each side there is already a room
      if (_takenPositions.Contains(checkingPos + Vector2.right))
        neighbors++;

      if (_takenPositions.Contains(checkingPos + Vector2.left))
        neighbors++;

      if (_takenPositions.Contains(checkingPos + Vector2.up))
        neighbors++;

      if (_takenPositions.Contains(checkingPos + Vector2.down))
        neighbors++;

      return neighbors;
    }

    private Vector2 SelectiveNewPosition()
    {
      // method differs from the above in the two commented ways
      var iteration = 0;
      var x = 0;
      var y = 0;
      var checkingPos = Vector2.zero;
      while (_takenPositions.Contains(checkingPos) || x >= _gridSizeX || x < -_gridSizeX || y >= _gridSizeY ||
             y < -_gridSizeY)
      {
        iteration = 0;
        var index = 0;
        while (NumberOfNeighbors(_takenPositions[index]) > 1 && iteration < 100)
        {
          //instead of getting a room to find an adject empty space, we start with one that only 
          //as one neighbor. This will make it more likely that it returns a room that branches out
          index = Mathf.RoundToInt(Random.value * (_takenPositions.Count - 1));
          iteration++;
        }

        x = (int) _takenPositions[index].x;
        y = (int) _takenPositions[index].y;
        var up = (Random.value < 0.5f);
        var positive = (Random.value < 0.5f);
        if (up)
        {
          y += positive ? 1 : -1;
        }
        else
        {
          x += positive ? 1 : -1;
        }

        checkingPos = new Vector2(x, y);
      }

      if (iteration >= 100)
      {
        throw new ArithmeticException($"Wasn't able to create NEW room pos within {iteration} iterations");
      }

      return checkingPos;
    }

    private void SetRoomDoors()
    {
      for (var x = 0; x < _gridSizeX * 2; x++)
      {
        for (var y = 0; y < _gridSizeY * 2; y++)
        {
          if (rooms[x, y] == null)
            continue;

          rooms[x, y].doorDown = (rooms.SafeGet(x, y - 1) != null);
          rooms[x, y].doorUp = (rooms.SafeGet(x, y + 1) != null);
          rooms[x, y].doorLeft = (rooms.SafeGet(x - 1, y) != null);
          rooms[x, y].doorRight = (rooms.SafeGet(x + 1, y) != null);
        }
      }
    }
  }

  public static class Utils
  {
    public static MazeRoomSetup SafeGet(this MazeRoomSetup[,] rooms, int x, int y)
    {
      if (x < 0 || rooms.GetLength(0) <= x || y < 0 || rooms.GetLength(1) <= y)
        return null;
      return rooms[x, y];
    }
  }
}