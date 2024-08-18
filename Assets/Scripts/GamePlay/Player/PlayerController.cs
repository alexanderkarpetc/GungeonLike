using System.Collections;
using GamePlay.Common;
using GamePlay.Weapons;
using UnityEngine;

namespace GamePlay.Player
{
    public class PlayerController : MonoBehaviour
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

        public void Init()
        {
            _initializer.Init(_startingWeapon);
        }

        private void Update()
        {
            ReadInput();
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

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            Vector2 targetVelocity = new Vector2(_horizontalMove, _verticalMove).normalized * StaticData.PlayerSpeedBase * AppModel.Player().SpeedMultiplier;
            _rigidbody.velocity = Vector2.Lerp(_rigidbody.velocity, targetVelocity, Inertia);
        }

        public void Hit()
        {
            if (!_isInvincible)
                StartCoroutine(ApplyHit());
        }

        private IEnumerator ApplyHit()
        {
            _isInvincible = true;
            var state = AppModel.Player();
            state.DealDamage();

            if (state.GetHp() <= 0)
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