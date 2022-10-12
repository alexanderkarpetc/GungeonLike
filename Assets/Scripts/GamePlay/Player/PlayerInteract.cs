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
    private PlayerLookDirection _direction;

    private void Start()
    {
      _layerMask = LayerMask.GetMask("Interactable");
      StartCoroutine(FindInteractable());
    }

    private void Update()
    {
      CheckInteract();
    }

    public Interactable GetInteractable()
    {
      return _interactable;
    }
    private IEnumerator FindInteractable()
    {
      while (true)
      {
        UpdatePlayerLookDirection();
        var interactables = Physics2D.OverlapCircleAll(transform.position, 0.5f, _layerMask)
          .Where(x => x.GetComponent<Interactable>().IsActive).Where(x =>
          {
            Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            var playerAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Vector2 interactableDirection = x.transform.position - transform.position;
            var interactableAngle = Mathf.Atan2(interactableDirection.y, interactableDirection.x) * Mathf.Rad2Deg;
            return Mathf.Abs(playerAngle - interactableAngle) < 90;
          }).ToList();
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

    public PlayerLookDirection GetPlayerLookDirection()
    {
      return _direction;
    }

    private void UpdatePlayerLookDirection()
    {
      Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
      var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
      _direction=  angle switch
      {
        < 45 and > -45 => PlayerLookDirection.Right,
        > 45 and < 135 => PlayerLookDirection.Up,
        < -45 and > -135 => PlayerLookDirection.Down,
        _ => PlayerLookDirection.Left
      };
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

  public enum PlayerLookDirection
  {
    Up,
    Down,
    Right,
    Left
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