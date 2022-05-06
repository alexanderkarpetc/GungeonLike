using System.Linq;
using GamePlay.Common;
using GamePlay.Weapons;
using UnityEngine;

namespace GamePlay.Enemy.Brain.Parts
{
  public class CubulonBotShooting : BotPart
  {
    protected float _nextShotTime;
    protected GameObject _projectile;
    private float _delay;

    public CubulonBotShooting(BotBrain brain) : base(brain)
    {
      _delay = brain.EnemyController.GetComponent<Animator>().runtimeAnimatorController.animationClips.ToList()
        .Find(x => x.name.Equals("DownLeft")).length;
      _nextShotTime = Time.time + _delay;
      _projectile = Resources.Load<GameObject>("Prefabs/Projectiles/BlueProjectile");
    }

    public override void OnUpdate()
    {
      if (Time.time > _nextShotTime)
      {
        _nextShotTime = Time.time + _delay;
        var angleShift = 360 / StaticData.EnemyCubulonShotsCount;
        for (var i = 0; i < StaticData.EnemyCubulonShotsCount; i++)
        {
          var go = GameObject.Instantiate(_projectile, Brain.EnemyController.transform.position, Quaternion.identity);
          go.transform.SetParent(AppModel.BulletContainer().transform);
          var projectile = go.GetComponent<Projectile>();
          projectile.Speed = StaticData.EnemyCubulonShotSpeed;
          projectile.Direction = Weapon.DegreeToVector2(i * angleShift);
        }
      }
    }
  }
}