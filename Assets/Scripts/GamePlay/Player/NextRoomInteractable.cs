using UnityEngine;

namespace GamePlay.Player
{
  public class NextRoomInteractable : Interactable
  {
    public override void Interact(PlayerInteract playerInteract)
    {
      AppModel.StraightRoomController.ProcessNextRoom();
    }
  }
}