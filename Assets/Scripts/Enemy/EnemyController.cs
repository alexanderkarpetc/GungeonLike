using System.Collections;
using Enemy.State;
using Pathfinding;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Enemy
{
  public class EnemyController : MonoBehaviour
  {
    [SerializeField] private AIDestinationSetter _destinationSetter;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private AIPath _aiPath;
    [SerializeField] private float speed;
    [SerializeField] private Animator _animator;
    [SerializeField] private BulletEnemyTurnAnimator _turnAnimator;
    [SerializeField] private float _hitAnimDuration;

    private EnemyState State;

    private void Start()
    {
      _destinationSetter.target = GameObject.Find("Player").transform;
      _aiPath.maxSpeed = speed;
      State = new EnemyState();
    }

    public void Hit(float damage, Vector2 impulse)
    {
      State.Hp -= damage;
      StartCoroutine(HitImpulse(impulse));
      if (!_turnAnimator.IsDying && State.Hp <= 0)
      {
        StopProcesses();
        _turnAnimator.IsDying = true;
        _animator.SetTrigger(EnemyAnimState.die);
        Invoke(nameof(DestroyView), 1);
        return;
      }

      StartCoroutine(HitAnimation());
    }

    private IEnumerator HitImpulse(Vector2 impulse)
    {
      _aiPath.canMove = false;
      _rigidbody.AddForce(impulse, ForceMode2D.Impulse);
      yield return new WaitForSeconds(0.3f);
      _aiPath.canMove = true;
    }

    private IEnumerator HitAnimation()
    {
      _turnAnimator.isAnimating = true;
      var angle = _turnAnimator.TurnAngle();
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
      
      yield return new WaitForSeconds(_hitAnimDuration);
      _turnAnimator.isAnimating = false;
    }

    private void StopProcesses()
    {
      _aiPath.maxSpeed = 0;
      Destroy(GetComponent<CircleCollider2D>());
    }

    public void DestroyView()
    {
      Destroy(gameObject);
    }
  }
}