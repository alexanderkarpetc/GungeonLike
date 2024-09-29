using System.Collections.Generic;
using GamePlay.Level.Controllers;
using GamePlay.UI;
using UnityEngine;

namespace GamePlay
{
  public class StraightLevelController : MonoBehaviour
  {
    [SerializeField] private List<GameObject> predefinedRooms;
    [SerializeField] private CameraFade _cameraFade;

    private int _currentRoomIndex = -1;
    private GameObject _currentRoom;
    private GameObject _astarObj;

    public void Init()
    {
      AppModel.StraightRoomController = this;
    }

    public void ProcessNextRoom()
    {
      _cameraFade.MakeFade();
      _currentRoomIndex++;
      if (_currentRoom != null)
      {
        Destroy(_currentRoom);
        foreach (var item in AppModel.DropManager().GetDropped)
        {
          Destroy(item.gameObject);
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