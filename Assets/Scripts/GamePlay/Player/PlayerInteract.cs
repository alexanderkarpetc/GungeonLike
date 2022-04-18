using UnityEngine;

namespace GamePlay.Player
{
  public class PlayerInteract : MonoBehaviour
  {
    public Interactable Interactable;
    [SerializeField] private GameObject _interactObj;

    private void Update()
    {
      CheckInteract();
    }

    private void CheckInteract()
    {
      _interactObj.SetActive(Interactable != null);
      if (Input.GetKey(KeyCode.E))
        Interactable?.Interact(this);
    }
  }

  public abstract class Interactable
  {
    public abstract void Interact(PlayerInteract playerInteract);
  }
}