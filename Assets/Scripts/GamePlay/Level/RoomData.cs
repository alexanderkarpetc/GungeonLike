using System.Collections.Generic;
using UnityEngine;

namespace GamePlay.Level
{
  public class RoomData : MonoBehaviour
  {
    public List<Transform> points;
    public List<NextRoomDoor> exits;
  }
}