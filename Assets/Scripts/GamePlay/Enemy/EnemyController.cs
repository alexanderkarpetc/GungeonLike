using System;
using System.Collections;
using System.Collections.Generic;
using GamePlay.Common;
using GamePlay.Enemy.Brain;
using GamePlay.Enemy.State;
using GamePlay.Weapons;
using Pathfinding;
using UnityEngine;

namespace GamePlay.Enemy
{
  public class EnemyController : MonoBehaviour
  {
    public static Func<EnemyType, GameObject, BotBrain> BotBrainByType = (type, gameObj) =>
    {
      if (type == EnemyType.Cubulon)
        return new CubulonBrain(gameObj);
      if (type == EnemyType.WormEnemy)
        return new WormBossBrain(gameObj);

      return new BotBrain(gameObj);
    };
    [SerializeField] protected EnemyType Type;
    [SerializeField] private AIDestinationSetter _destinationSetter;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private AIPath _aiPath;
    [SerializeField] private Animator _animator;
    [SerializeField] private BulletEnemyTurnAnimator _turnAnimator;
    [SerializeField] private float _hitAnimDuration;

    public EnemyState State;
    private BotBrain _botBrain;
    public Weapon Weapon;
    public Action<EnemyController> OnDeath;
    private void Start()
    {
      State = new EnemyState();
      _botBrain = BotBrainByType(Type, gameObject);
      _botBrain.OnCreate();
    }

    private void Update()
    {
      if(!_turnAnimator.IsDying)
        _botBrain.OnUpdate();
    }

    public void Hit(float damage, Vector2 impulse)
    {
      State.Hp -= damage;
      StartCoroutine(HitImpulse(impulse));
      if (!_turnAnimator.IsDying && State.Hp <= 0)
      {
        Death();
        return;
      }

      StartCoroutine(HitAnimation());
    }

    private void Death()
    {
      StopProcesses();
      _turnAnimator.IsDying = true;
      _animator.SetTrigger(EnemyAnimState.die);
      Invoke(nameof(DestroyView), 1);
      AppModel.Drop().CheckDrop(transform, 10);
      OnDeath?.Invoke(this);
    }

    private IEnumerator HitImpulse(Vector2 impulse)
    {
      _aiPath.canMove = false;
      _rigidbody.AddForce(impulse, ForceMode2D.Impulse);
      yield return new WaitForSeconds(_hitAnimDuration);
      _aiPath.canMove = true;
    }

    protected virtual IEnumerator HitAnimation()
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

    public AIDestinationSetter GetDestinationSetter()
    {
      return _destinationSetter;
    }

    public AIPath GetAiPath()
    {
      return _aiPath;
    }
  }
}