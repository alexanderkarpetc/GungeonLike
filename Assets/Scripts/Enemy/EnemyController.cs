using Enemy.State;
using Pathfinding;
using UnityEngine;

namespace Enemy
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private AIDestinationSetter _destinationSetter;
        [SerializeField] private AIPath _aiPath;
        [SerializeField] private float speed;
        [SerializeField] private Animator _animator;
        [SerializeField] private BulletEnemyTurnAnimator _turnAnimator;

        private EnemyState State;

        private void Start()
        {
            _destinationSetter.target = GameObject.Find("Player").transform;
            _aiPath.maxSpeed = speed;
            State = new EnemyState();
        }

        void Update()
        {
        
        }

        public void Hit(float damage)
        {
            State.Hp -= damage;
            if (!_turnAnimator.IsDying && State.Hp <= 0)
            {
                StopProcesses();
                _turnAnimator.IsDying = true;
                _animator.SetTrigger(EnemyAnimState.die);
                Invoke(nameof(DestroyView), 1.5f);
            }
        }

        private void StopProcesses()
        {
            _aiPath.maxSpeed = 0;
        }

        public void DestroyView()
        {
            Destroy(gameObject);
        }
    }
}
