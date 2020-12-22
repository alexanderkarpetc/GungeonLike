using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using GamePlay.Level;
using GamePlay.Level.Controllers;
using GamePlay.Player;
using UnityEngine;

namespace GamePlay
{
  public class StraightLevelController : MonoBehaviour
  {
    [SerializeField] private List<GameObject> predefinedRooms;
    private int _currentRoomIndex = -1;
    private GameObject _currentRoom;
    private GameObject _astarObj;

    private void Start()
    {
      AppModel.StraightRoomController = this;
    }

    public void ProcessNextRoom()
    {
      _currentRoomIndex++;
      if (_currentRoom != null)
      {
        Destroy(_currentRoom);
        foreach (Transform child in AppModel.Drop().GetDropped())
        {
          Destroy(child.gameObject);
        }
      }
      var nextRoom = predefinedRooms[_currentRoomIndex];
      _currentRoom = Instantiate(nextRoom);
      _currentRoom.AddComponent<StraightRoomController>();
      AppModel.PlayerTransform().position = _currentRoom.transform.Find("StartPoint").position;
    }

    public void ReInitAstar()
    {
      if (_astarObj == null)
      {
        var astarPrefab = Resources.Load<GameObject>("Prefabs/Util/AStar");
        _astarObj = Instantiate(astarPrefab);
      }
      else
      {
        _astarObj.GetComponent<AstarPath>().Scan();
      }
    }
  }
}