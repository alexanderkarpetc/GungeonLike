using System;
using GamePlay.Player;
using UnityEngine;

namespace GamePlay.Level
{
  public class NextRoomDoor : MonoBehaviour
  {
    [HideInInspector] public bool CanGoNextRoom;
    private NextRoomInteractable _interactable;

    private void Start()
    {
      _interactable = new NextRoomInteractable();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
      if(!CanGoNextRoom)
        return;
      if (other.CompareTag("Player"))
      {
        other.GetComponent<PlayerInteract>().Interactable = _interactable;
      }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
      if(!CanGoNextRoom)
        return;
      if (other.CompareTag("Player"))
      {
        other.GetComponent<PlayerInteract>().Interactable = null;
      }
    }
  }
}