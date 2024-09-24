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
        [SerializeField] private Collider2D _collider;
        [SerializeField] private DynamicGridObstacle _gridObstacle;
        [SerializeField] protected Animator _animator;
        
        private static readonly int RollUp = Animator.StringToHash("RollUp");
        private static readonly int RollDown = Animator.StringToHash("RollDown");
        private static readonly int RollRight = Animator.StringToHash("RollRight");
        private static readonly int RollLeft = Animator.StringToHash("RollLeft");
        
        private bool _isCarrying;
        private Coroutine _routine;
        private Vector2 _carryOffset;

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
            _rigidbody2D.linearVelocity = direction * kickPower;
            _env.gameObject.AddComponent<DestroyOnTouch>();
            PlayKickAnimation(lookDirection);
        }

        private void PlayKickAnimation(PlayerLookDirection lookDirection)
        {
            if (_animator != null)
            {
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
        }

        private void PickUp()
        {
            var playerTransform = AppModel.PlayerTransform();
            var playerWeaponTurn = playerTransform.GetComponent<PlayerWeaponTurn>();

            playerWeaponTurn._leftHand.gameObject.SetActive(false);
            
            var playerShooting = AppModel.PlayerGameObj().GetComponent<PlayerShooting>();
            playerShooting.enabled = false;
            playerShooting.Weapon.gameObject.SetActive(false);

            _isCarrying = true;
            _rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
            _collider.enabled = false;
            _gridObstacle.enabled = false;

            // Calculate offset from the player (instead of changing the parent)
            _carryOffset = Vector2.up / 2;
            _routine = StartCoroutine(UpdatePosition(playerTransform));
        }

        private IEnumerator UpdatePosition(Transform playerTransform)
        {
            while (_isCarrying)
            {
                yield return null;

                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 direction = (mousePosition - (Vector2)playerTransform.position).normalized;
                
                // Update the object's position relative to the player, without setting it as a child
                _env.transform.position = (Vector2)playerTransform.position + direction * _carryOffset.magnitude;
            }
        }

        private void PutDown()
        {
            var playerTransform = AppModel.PlayerTransform();
            var playerWeaponTurn = playerTransform.GetComponent<PlayerWeaponTurn>();

            playerWeaponTurn._leftHand.gameObject.SetActive(true);

            var playerShooting = AppModel.PlayerGameObj().GetComponent<PlayerShooting>();
            playerShooting.enabled = true;
            playerShooting.Weapon.gameObject.SetActive(true);

            _isCarrying = false;
            _rigidbody2D.bodyType = RigidbodyType2D.Static;
            _collider.enabled = true;
            _gridObstacle.enabled = true;

            if (_routine != null)
            {
                StopCoroutine(_routine);
            }
        }
    }
}