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
    [SerializeField] protected Animator _animator;
    private static readonly int RollUp = Animator.StringToHash("RollUp");
    private static readonly int RollDown = Animator.StringToHash("RollDown");
    private static readonly int RollRight = Animator.StringToHash("RollRight");
    private static readonly int RollLeft = Animator.StringToHash("RollLeft");
    private bool _isCarrying;
    private Coroutine _routine;
    
    public override void Interact(PlayerInteract playerInteract)
    {
      if (!_isCarrying)
        PickUp();
      else
        PutDown();
    }
    
    public void Kick(Vector2 direction, float kickPower, PlayerLookDirection lookDirection)
    {
      _rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
      _rigidbody2D.velocity = direction * kickPower;
      _env.gameObject.AddComponent<DestroyOnTouch>();
      if(_animator != null)
        switch (lookDirection)
        {
          case PlayerLookDirection.Up:
            _animator.SetTrigger(RollUp);
            break;
          case PlayerLookDirection.Down:
            _animator.SetTrigger(RollDown);
            break;
          case PlayerLookDirection.Right:
            _animator.SetTrigger(RollRight);
            break;
          case PlayerLookDirection.Left:
            _animator.SetTrigger(RollLeft);
            break;
        }
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