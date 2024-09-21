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
        [SerializeField] private Weapon _startingWeapon;
        
        private int _verticalMove;
        private int _horizontalMove;
        private bool _isInvincible;
        private PlayerInitializer _initializer = new PlayerInitializer();
        
        private readonly float Inertia = 0.2f;

        // todo: move to some init
        private void Start()
        {
            if (!IsOwner) return; // Only initialize for the owning player

            // Initialize the player if this is the owner
            AppModel.SetPlayerGo(gameObject);
            _initializer.Init(_startingWeapon);
            // todo: should be changed to some logic
        }

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

        public void Hit()
        {
            if (!_isInvincible)
                StartCoroutine(ApplyHit());
        }

        private IEnumerator ApplyHit()
        {
            _isInvincible = true;
            var state = AppModel.PlayerState();
            state.DealDamage();

            if (state.CurrentHp <= 0)
            {
                Die();
                yield break;
            }

            // ScreenBlink
            var bodyColor = _body.color;
            bodyColor.a = 0.5f;
            _body.color = bodyColor;

            yield return new WaitForSeconds(1f);

            bodyColor.a = 1f;
            _body.color = bodyColor;
            _isInvincible = false;
        }

        private void Die()
        {
            Destroy(gameObject);
        }
    }
}