using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace GamePlay.Player
{
    public class PlayerAnimatorView : NetworkBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] protected GameObject _body;
        [HideInInspector] public int VerticalMove;
        [HideInInspector] public int HorizontalMove;
        [HideInInspector] public bool IsKicking;

        private Dictionary<PlayerViewDirection, PlayerAnimationSet> _animationSets = new Dictionary<PlayerViewDirection, PlayerAnimationSet>
        {
            {PlayerViewDirection.Up, new PlayerAnimationSet{Idle = PlayerAnimState.Up, Run = PlayerAnimState.UpRun, Kick = PlayerAnimState.UpKick, Scale = 1}},
            {PlayerViewDirection.Down, new PlayerAnimationSet{Idle = PlayerAnimState.Down, Run = PlayerAnimState.DownRun, Kick = PlayerAnimState.DownKick, Scale = 1}},
            {PlayerViewDirection.RightUp, new PlayerAnimationSet{Idle = PlayerAnimState.UpRight, Run = PlayerAnimState.UpRightRun, Kick = PlayerAnimState.UpKick, Scale = 1}},
            {PlayerViewDirection.RightDown, new PlayerAnimationSet{Idle = PlayerAnimState.DownRight, Run = PlayerAnimState.DownRightRun, Kick = PlayerAnimState.DownKick, Scale = 1}},
            {PlayerViewDirection.LeftUp, new PlayerAnimationSet{Idle = PlayerAnimState.UpRight, Run = PlayerAnimState.UpRightRun, Kick = PlayerAnimState.UpKick, Scale = -1}},
            {PlayerViewDirection.LeftDown, new PlayerAnimationSet{Idle = PlayerAnimState.DownRight, Run = PlayerAnimState.DownRightRun, Kick = PlayerAnimState.DownKick, Scale = -1}},
        };
        
        private PlayerViewDirection _currentViewDirection;
        private bool _wasRunning;
        private bool _wasKicking;

        private void Update()
        {
            if (IsOwner)  // Only allow the local player to control the animations
            {
                Animate();
                UpdateAnimationStateServerRpc(_currentViewDirection, HorizontalMove, VerticalMove, IsKicking);  // Sync state across the network
            }
        }

        [ServerRpc]
        private void UpdateAnimationStateServerRpc(PlayerViewDirection direction, int horizontalMove, int verticalMove, bool isKicking)
        {
            // Broadcast the animation state to other clients
            UpdateAnimationClientRpc(direction, horizontalMove, verticalMove, isKicking);
        }

        [ClientRpc]
        private void UpdateAnimationClientRpc(PlayerViewDirection direction, int horizontalMove, int verticalMove, bool isKicking)
        {
            // Update the animation state for all clients
            HorizontalMove = horizontalMove;
            VerticalMove = verticalMove;
            IsKicking = isKicking;
            PickAnimation(direction);  // Ensure animations get updated on all clients
        }

        private void Animate()
        {
            Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            if (angle >= -75 && angle < 25)
            {
                PickAnimation(PlayerViewDirection.RightDown);
                return;
            }

            if (angle >= 25 && angle < 70)
            {
                PickAnimation(PlayerViewDirection.RightUp);
                return;
            }

            if (angle >= 70 && angle < 110)
            {
                PickAnimation(PlayerViewDirection.Up);
                return;
            }

            if (angle >= 110 && angle < 155)
            {
                PickAnimation(PlayerViewDirection.LeftUp);
                return;
            }

            if (angle > -135 && angle < -75)
            {
                PickAnimation(PlayerViewDirection.Down);
                return;
            }

            if (Mathf.Abs(angle) >= 135)
            {
                PickAnimation(PlayerViewDirection.LeftDown);
                return;
            }
        }

        private void PickAnimation(PlayerViewDirection viewDirection)
        {
            var animationSet = _animationSets[viewDirection];
            var isRunning = HorizontalMove != 0 || VerticalMove != 0;
            if (_currentViewDirection == viewDirection && _wasRunning == isRunning && _wasKicking == IsKicking)
                return;
            _currentViewDirection = viewDirection;
            _wasRunning = isRunning;
            _wasKicking = IsKicking;

            if (IsKicking)
                _animator.SetTrigger(animationSet.Kick);
            else if (isRunning)
                _animator.SetTrigger(animationSet.Run);
            else
                _animator.SetTrigger(animationSet.Idle);

            SpriteUtil.SetXScale(_body, animationSet.Scale);
        }

        private class PlayerAnimationSet
        {
            public int Idle;
            public int Run;
            public int Kick;
            public int Scale;
        }

        private enum PlayerViewDirection
        {
            Up,
            Down,
            LeftUp,
            LeftDown,
            RightDown,
            RightUp,
        }
    }
}