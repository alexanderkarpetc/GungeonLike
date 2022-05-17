using System;
using System.Collections;
using System.Linq;
using GamePlay.Common;
using GamePlay.Weapons;
using UnityEngine;

namespace GamePlay.Enemy.Brain.Parts
{
  public class GunKnightAttacking : BotPart
  {
    private GameObject _projectile;
    private bool _isAttacking;
    private readonly KnightEnemyAnimator _animator;
    private float _nextAttackTime = 0;
    private float _attackInterval = 2f;

    public GunKnightAttacking(BotBrain brain) : base(brain)
    {
      _projectile = Resources.Load<GameObject>("Prefabs/Projectiles/BlueProjectile");
      _animator = Brain.EnemyController.GetComponent<KnightEnemyAnimator>();
    }
    
    protected override void OnUpdate()
    {
      if(Brain.Target == null || _isAttacking || _nextAttackTime > Time.time)
        return;
      var raycast = Physics2D.LinecastAll(Brain.Owner.transform.position, AppModel.PlayerGameObj().transform.position)
        .Where(x=>x.collider.CompareTag("Obstacle") || x.collider.CompareTag("Player"));
      if (!raycast.First().collider.CompareTag("Player"))
        return;
      StartAttack();
    }

    private void StartAttack()
    {
      Brain.EnemyController.GetAiPath().maxSpeed = 0;
      _isAttacking = true;
      _animator.StartAttack();
      DelayCall(Hit, 1.2f);
      _nextAttackTime = Time.time + _attackInterval;
    }


    private void Hit()
    {
      var angleShift = 360 / StaticData.EnemyCubulonShotsCount;
      for (var i = 0; i < StaticData.EnemyCubulonShotsCount; i++)
      {
        var go = GameObject.Instantiate(_projectile, Brain.EnemyController.transform.position, Quaternion.identity);
        go.transform.SetParent(AppModel.BulletContainer().transform);
        var projectile = go.GetComponent<Projectile>();
        projectile.Speed = StaticData.EnemyCubulonShotSpeed;
        projectile.Direction = Weapon.DegreeToVector2(i * angleShift);
      }
      
      _isAttacking = false;
      _animator.StopAttacking();
      Brain.EnemyController.GetAiPath().maxSpeed = StaticData.EnemyKnightSpeedBase;
    }
  }
}