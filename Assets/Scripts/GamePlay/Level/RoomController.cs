using System;
using UnityEngine;

namespace GamePlay.Level
{
  public class RoomController : MonoBehaviour
  {
    public RoomSetup setup;
    public RoomState State;
    
    public RoomController()
    {
      State = new RoomState();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
      if (!State.IsCleaned && !State.IsVisited)
      {
        CloseDoors();
        State.IsVisited = true;
      } 
    }

    private void CloseDoors()
    {
      Debug.Log("Closing doors");
      Debug.Log(setup);
    }
  }
}