using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GamePlay.Common;
using GamePlay.Enemy.Brain;
using GamePlay.Enemy.State;
using GamePlay.Weapons;
using Pathfinding;
using Unity.Netcode;
using UnityEngine;

namespace GamePlay.Enemy
{
  public class EnemyController : NetworkBehaviour
  {
    public static Func<EnemyType, GameObject, BotBrain> BotBrainByType = (type, gameObj) =>
    {
      if (type == EnemyType.Cubulon)
        return new CubulonBrain(gameObj);
      if (type == EnemyType.GrenadeMan)
        return new GrenadeBrain(gameObj);
      if (type == EnemyType.WormEnemy)
        return new WormBossBrain(gameObj);
      if (type == EnemyType.Sniper)
        return new SniperBulletBrain(gameObj);
      if (type == EnemyType.GunKnight)
        return new GunKnightBrain(gameObj);

      return new BotBrain(gameObj);
    };
    [SerializeField] public EnemyType Type;
    [SerializeField] private AIDestinationSetter _destinationSetter;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private AIPath _aiPath;
    [SerializeField] private EnemyAnimator enemyAnimator;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _hitAnimDuration;

    public Action<EnemyController> OnDeath;

    public NetworkVariable<EnemyState> State = new();
    public Weapon Weapon;
    public Vector3? CurrentTarget;
    
    private BotBrain _botBrain;
    private bool isDying;

    private void Start()
    {
      if(!IsSpawned)
        GetComponent<NetworkObject>().Spawn();
      if (!IsServer) return;
      //todo: probably should be reworked
      Weapon.IsOwner = true;
      SetHealthServerRpc(10);
      _botBrain = BotBrainByType(Type, gameObject);
      _botBrain.OnCreate();
    }

    private void Update()
    {
      if (!IsServer) return;
      if(!isDying)
        _botBrain.Update();
    }

    [ServerRpc]
    public void SetHealthServerRpc(int value)
    {
      State.Value = new EnemyState{Hp = value};
    }
    
    [ServerRpc]
    public void DealDamageServerRpc(float damage)
    {
      State.Value = new EnemyState {Hp = State.Value.Hp - damage};
    }

    public void Hit(Vector2 impulse)
    {
      StartCoroutine(HitImpulse(impulse));
      if (!isDying && State.Value.Hp <= 0)
      {
        Death();
        return;
      }

      enemyAnimator.Hit(_hitAnimDuration);
    }

    private void Death()
    {
      StopProcesses();
      isDying = true;
      enemyAnimator.Die();
      var deathDuration = _animator.runtimeAnimatorController.animationClips.First(x=>x.name.Equals("Death")).averageDuration;
      Invoke(nameof(AfterDeath), deathDuration);
    }

    private void AfterDeath()
    {
      OnDeath?.Invoke(this);
      AppModel.DropManager().DropOnEnemyDeath(transform, Type);
      AppModel.PlayerState().AddExp(Type);
      Destroy(gameObject);
    }

    private IEnumerator HitImpulse(Vector2 impulse)
    {
      _aiPath.canMove = false;
      _rigidbody.AddForce(impulse, ForceMode2D.Impulse);
      yield return new WaitForSeconds(_hitAnimDuration);
      _aiPath.canMove = true;
    }

    private void StopProcesses()
    {
      _aiPath.maxSpeed = 0;
      Destroy(GetComponent<CircleCollider2D>());
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