using Pathfinding;
using UnityEngine;

namespace GamePlay.Enemy
{
    public class KnightEnemyAnimator : EnemyAnimator
    {
        [SerializeField] private AIPath _aiPath;

        private bool _attacking;

        private void Update()
        {
            if(_attacking)
                return;
            Animate();
        }

        public void StartAttack()
        {
            _attacking = true;
            var angle = TurnAngle(_aiPath.steeringTarget);
            if (angle >= -90 && angle < 10)
            {
                _animator.SetTrigger(EnemyAnimState.hitRightDown);
            }
            else if (angle >= 10 && angle < 90)
            {
                _animator.SetTrigger(EnemyAnimState.hitRightUp);
            }
            else if (angle >= 90 && angle < 170)
            {
                _animator.SetTrigger(EnemyAnimState.hitLeftUp);
            }
            else
            {
                _animator.SetTrigger(EnemyAnimState.hitLeftDown);
            }

            _currentAnim = 0;
        } 

        public void StopAttacking()
        {
            _attacking = false;
        }

        private void Animate()
        {
            var angle = TurnAngle(_aiPath.steeringTarget);
      
            if (angle >= 10 && angle < 170)
            {
                ProcessAnimation(EnemyAnimState.up);
                return;
            }
            ProcessAnimation(EnemyAnimState.down);
        }
    }
}