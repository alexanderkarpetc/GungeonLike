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
      _projectile = Resources.Load<GameObject>("Prefabs/Projectiles/Projectile");
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
      var playerPos = AppModel.PlayerTransform().position;
      var center = Brain.EnemyController.transform.position - (playerPos - Brain.EnemyController.transform.position).normalized * 3;
      var radius = 4;

      for (var i = 0; i < StaticData.GunKnightShotsCount; i++)
      {
        Vector2 direction = playerPos - center;
        var flyAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        var angle = flyAngle - StaticData.GunKnightShotsCount + i*2;
        var x = Mathf.Cos(angle * Mathf.Deg2Rad) * radius + center.x;
        var y = Mathf.Sin(angle * Mathf.Deg2Rad) * radius + center.y;
        var spawnPos = new Vector3(x, y, 0);
        var go = GameObject.Instantiate(_projectile, spawnPos, Quaternion.identity);
        go.transform.SetParent(AppModel.BulletContainer().transform);
        var projectile = go.GetComponent<Projectile>();
        projectile.Speed = 13;
        projectile.Direction = Weapon.DegreeToVector2(flyAngle - 15 + i);
      }
      
      _isAttacking = false;
      _animator.StopAttacking();
      Brain.EnemyController.GetAiPath().maxSpeed = StaticData.EnemyKnightSpeedBase;
    }
  }
}