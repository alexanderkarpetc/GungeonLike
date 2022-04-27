using GamePlay.Level;

namespace GamePlay.Player
{
    public class TableInteractable : Interactable
    {
        public Table Table;

        public override void Interact(PlayerInteract playerInteract)
        {
            Table.Flip();
            playerInteract.Interactable = null;
        }
    }
}