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