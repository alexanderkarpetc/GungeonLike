using UnityEngine;

namespace GamePlay.Player
{
  public class NextRoomInteractable : Interactable
  {
    public override void Interact(GameObject interactObj)
    {
      AppModel.StraightRoomController.ProcessNextRoom();
    }
  }
}