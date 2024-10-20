using UnityEngine;
using UnityEngine.Serialization;

namespace GamePlay.Player
{
  public class NextRoomInteractable : Interactable
  {
    public DoorType DoorType;
    public override void Interact(PlayerInteract playerInteract)
    {
      AppModel.LevelController.ProcessNextRoom();
    }

    protected override void OnStart()
    {
      AppModel.RoomController.Doors.Add(this);
    }
  }
  public enum DoorType
  {
      FrontDoor1,
      SideDoor1,
  }
}