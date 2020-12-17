using UnityEngine;

namespace GamePlay.Player
{
  public class PlayerInteract : MonoBehaviour
  {
    public Interactable Interactable;

    private void Update()
    {
      CheckInteract();
    }

    private void CheckInteract()
    {
      if (Input.GetKey(KeyCode.E))
        Interactable?.Interact();
    }
  }

  public abstract class Interactable
  {
    public abstract void Interact();
  }
}