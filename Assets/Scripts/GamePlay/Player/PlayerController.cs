using System.Collections;
using GamePlay.Common;
using GamePlay.Weapons;
using Unity.Netcode;
using UnityEngine;

namespace GamePlay.Player
{
    public class PlayerController : NetworkBehaviour
    {
        [HideInInspector] public bool IsBusy;

        [SerializeField] private PlayerAnimatorView playerAnimatorView;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private SpriteRenderer _body;
        
        private int _verticalMove;
        private int _horizontalMove;
        
        private readonly float Inertia = 0.2f;
        private bool _isDamaging;

        private void Update()
        {
            if (!IsOwner) return; // Only the owner should handle input
            ReadInput();
        }

        private void FixedUpdate()
        {
            if (!IsOwner) return; // Only the owner can move their player
            Move();
        }

        private void ReadInput()
        {
            if (IsBusy)
            {
                _horizontalMove = 0;
                _verticalMove = 0;
            }
            else
            {
                _horizontalMove = Input.GetKey(KeyCode.D) ? 1 : (Input.GetKey(KeyCode.A) ? -1 : 0);
                _verticalMove = Input.GetKey(KeyCode.W) ? 1 : (Input.GetKey(KeyCode.S) ? -1 : 0);
            }

            playerAnimatorView.HorizontalMove = _horizontalMove;
            playerAnimatorView.VerticalMove = _verticalMove;
        }

        private void Move()
        {
            Vector2 targetVelocity = new Vector2(_horizontalMove, _verticalMove).normalized * StaticData.PlayerSpeedBase * AppModel.PlayerState().SpeedMultiplier;
            _rigidbody.linearVelocity = Vector2.Lerp(_rigidbody.linearVelocity, targetVelocity, Inertia);
        }

            
        [ServerRpc(RequireOwnership = false)]
        public void DealDamageServerRpc(float damage, ulong ownerId)
        {
            var state = AppModel.PlayerState(ownerId);
            state.DealDamage(Mathf.CeilToInt(damage));
            if (state.CurrentHp.Value <= 0)
            {
                Debug.LogError("Player died");
                // Die();
                // return;
            }

            ApplyHitClientRpc(ownerId);
            StartCoroutine(ApplyHitAnimation());
        }

        [ClientRpc]
        private void ApplyHitClientRpc(ulong clientId)
        {
            if (NetworkManager.Singleton.LocalClientId == clientId)
            {
                if (_isDamaging)
                {
                    return;
                }
                StartCoroutine(ApplyHitAnimation());
            }
        }

        private IEnumerator ApplyHitAnimation()
        {
            _isDamaging = true;
            // Red Screen
            _body.color = Color.red;

            yield return new WaitForSeconds(0.3f);

            _body.color = Color.white;
            _isDamaging = false;
        }

        private void Die()
        {
            Destroy(gameObject);
        }
    }
}