﻿using System.Linq;
using GamePlay.Common;
using GamePlay.Weapons;
using UnityEngine;

namespace GamePlay.Enemy.Brain.Parts
{
  public class CubulonBotShooting : BotPart
  {
    private float _nextShotTime;
    private GameObject _projectile;
    private float _delay;
    private string _projectileName;

    public CubulonBotShooting(BotBrain brain) : base(brain)
    {
      _delay = brain.EnemyController.GetComponent<Animator>().runtimeAnimatorController.animationClips.ToList()
        .Find(x => x.name.Equals("DownLeft")).length;
      _nextShotTime = Time.time + _delay;
      _projectile = Resources.Load<GameObject>("Prefabs/Projectiles/BlueProjectile");
      _projectileName = _projectile.GetComponent<Projectile>().ProjectileName;
    }

    protected override void OnUpdate()
    {
      if (Time.time > _nextShotTime)
      {
        _nextShotTime = Time.time + _delay;
        var angleShift = 360 / StaticData.EnemyCubulonShotsCount;
        for (var i = 0; i < StaticData.EnemyCubulonShotsCount; i++)
        {
          var go = BulletPoolManager.Instance.GetBulletFromPool(_projectile, Brain.EnemyController.transform.position, Quaternion.identity, _projectileName);
          go.transform.SetParent(AppModel.BulletContainer().transform);
          var projectile = go.GetComponent<Projectile>();
          projectile.Speed = StaticData.EnemyCubulonShotSpeed;
          projectile.Direction = Weapon.DegreeToVector2(i * angleShift);
        }
      }
    }
  }
}