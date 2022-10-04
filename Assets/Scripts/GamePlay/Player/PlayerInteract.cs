using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace GamePlay.Player
{
  public class PlayerInteract : MonoBehaviour
  {
    private Interactable _interactable;
    [SerializeField] private GameObject _interactObj;
    private int _layerMask;

    private void Start()
    {
      _layerMask = LayerMask.GetMask("Interactable");
      StartCoroutine(FindInteractable());
    }

    private void Update()
    {
      CheckInteract();
    }

    private IEnumerator FindInteractable()
    {
      while (true)
      {
        var interactables = Physics2D.OverlapCircleAll(transform.position, 0.5f, _layerMask).Where(x=>x.GetComponent<Interactable>().IsActive).ToList();
        if (!interactables.Any())
        {
          yield return new WaitForSeconds(0.2f);
          _interactable = null;
          continue;
        }
        var closest = interactables.OrderByDescending(x => Vector2.Distance(transform.position, x.transform.position)).First();
        _interactable = closest.GetComponent<Interactable>();
        yield return new WaitForSeconds(0.2f);
      }
    }

    private void CheckInteract()
    {
      _interactObj.SetActive(_interactable != null);
      if (_interactable != null && Input.GetKeyDown(KeyCode.E))
      {
        _interactable.Interact(this);
        _interactable = null;
      }
    }
  }

  public abstract class Interactable : MonoBehaviour
  {
    public bool IsActive;

    private void Start()
    {
      gameObject.layer = LayerMask.NameToLayer("Interactable");
    }

    public abstract void Interact(PlayerInteract playerInteract);
  }
}