namespace GamePlay.Player
{
  public class NextRoomInteractable : Interactable
  {
    public override void Interact()
    {
      AppModel.StraightRoomController.ProcessNextRoom();
    }
  }
}