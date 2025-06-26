using UnityEngine;

namespace GamePlay.Enemy
{
    public class EnemyAnimator : MonoBehaviour
    {
        [SerializeField] protected Animator _animator;
        protected EnemyController _controller;
        protected int _currentAnim;

        public virtual void Hit(float hitAnimDuration){}

        protected float TurnAngle(Vector3 target)
        {
            Vector2 direction = target - transform.position;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            return angle;
        }
        
        private void Start()
        {
            _controller = GetComponent<EnemyController>();
        }

        public void Die()
        {
            _animator.SetTrigger(EnemyAnimState.die);
            Destroy(this);
        }
        
        protected void ProcessAnimation(int animation)
        {
            if (_currentAnim == animation)
                return;
            _currentAnim = animation;
            TurnTo();
        }

        protected void TurnTo()
        {
            _animator.SetTrigger(_currentAnim);
        }
    }
}