using System.Collections.Generic;
using GamePlay.Enemy;
using GamePlay.Player;
using UnityEngine;

namespace GamePlay.Level
{
  public enum RoomKind
  {
    Normal = 0,
    Boss = 1,
    Treasure = 2,
  }

  public class RoomData : MonoBehaviour
  {
    public List<Transform> points;
    public List<NextRoomInteractable> exits;
    public List<EnemyController> enemies;
    public List<Transform> controlPoints;
    public RoomKind kind;
  }
}