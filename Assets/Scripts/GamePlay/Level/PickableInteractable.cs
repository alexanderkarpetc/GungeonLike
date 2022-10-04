using System;
using System.Collections;
using GamePlay.Player;
using Pathfinding;
using UnityEngine;

namespace GamePlay.Level
{
  public class PickableInteractable : Interactable
  {
    [SerializeField] private Environment _env;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private Collider2D _colllider;
    [SerializeField] private DynamicGridObstacle _gridObstacle;
    private bool _isCarrying;
    private Coroutine _routine;

    public override void Interact(PlayerInteract playerInteract)
    {
      if (!_isCarrying)
        PickUp();
      else
        PutDown();
    }


    private void PickUp()
    {
      AppModel.PlayerTransform().Find("WeaponSlot").GetComponent<PlayerWeaponTurn>()._leftHand.gameObject.SetActive(false);
      var playerShooting = AppModel.PlayerGameObj().GetComponent<PlayerShooting>();
      playerShooting.enabled = false;
      playerShooting.Weapon.gameObject.SetActive(false);
      _isCarrying = true;
      _env.transform.SetParent(AppModel.PlayerTransform(), true);
      _rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
      _colllider.enabled = false;
      _gridObstacle.enabled = false;
      _routine = StartCoroutine(UpdatePos());
    }

    private IEnumerator UpdatePos()
    {
      while (true)
      {
        yield return null;
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - AppModel.PlayerTransform().position;
        _env.transform.localPosition = direction.normalized / 2;
      }
    }

    private void PutDown()
    {
      AppModel.PlayerTransform().Find("WeaponSlot").GetComponent<PlayerWeaponTurn>()._leftHand.gameObject.SetActive(true);
      var playerShooting = AppModel.PlayerGameObj().GetComponent<PlayerShooting>();
      playerShooting.enabled = true;
      playerShooting.Weapon.gameObject.SetActive(true);
      _isCarrying = false;
      _env.transform.SetParent(AppModel.EnvContainer().transform);
      _rigidbody2D.bodyType = RigidbodyType2D.Static;
      _colllider.enabled = true;
      _gridObstacle.enabled = true;
      StopCoroutine(_routine);
    }
  }
}