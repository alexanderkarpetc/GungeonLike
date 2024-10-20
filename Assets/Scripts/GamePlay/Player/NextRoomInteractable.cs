using UnityEngine;
using UnityEngine.Serialization;

namespace GamePlay.Player
{
  public class NextRoomInteractable : Interactable
  {
    public DoorType DoorType;
    public override void Interact(PlayerInteract playerInteract)
    {
      AppModel.StraightRoomController.ProcessNextRoom();
    }

  }
  public enum DoorType
  {
      FrontDoor1,
      SideDoor1,
  }
}