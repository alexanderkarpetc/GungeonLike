using GamePlay.Common;
using GamePlay.Enemy.Brain.Parts;
using GamePlay.Weapons;
using Unity.Netcode;
using UnityEngine;

namespace GamePlay.Enemy.Brain
{
  public class GunKnightBrain : BotBrain
  {
    public override void Init()
    {
      base.Init();
      _parts.Clear();
      var moving = new TargetFinder(this);
      var attacking = new GunKnightAttacking(this);
      _parts.Add(moving);
      _parts.Add(attacking);
      EnemyController.GetAiPath().maxSpeed = StaticData.EnemyKnightSpeedBase;
      // EnemyController.GetDestinationSetter().target = AppModel.PlayerTransform();
      EnemyController.SetHealthServerRpc(70);
    }

    [ClientRpc]
    protected override void ShootClientRpc()
    {
      var playerPos = AppModel.PlayerTransform().position;
      var center = EnemyController.transform.position - (playerPos - EnemyController.transform.position).normalized * 3;
      var radius = 4;
      ResourceLoader.Instance.GetCubulonResources(out var proj, out var projName);
      
      for (var i = 0; i < StaticData.GunKnightShotsCount; i++)
      {
        Vector2 direction = playerPos - center;
        var flyAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        var angle = flyAngle - StaticData.GunKnightShotsCount + i*2;
        var x = Mathf.Cos(angle * Mathf.Deg2Rad) * radius + center.x;
        var y = Mathf.Sin(angle * Mathf.Deg2Rad) * radius + center.y;
        var spawnPos = new Vector3(x, y, 0);
        var go = BulletPoolManager.Instance.GetBulletFromPool(proj, spawnPos, Quaternion.identity, projName);
        go.transform.SetParent(AppModel.BulletContainer().transform);
        var projectile = go.GetComponent<Projectile>();
        projectile.Speed = 13;
        projectile.Direction = Weapon.DegreeToVector2(flyAngle - 15 + i);
        projectile.IsOwner = true;
      }
    }
  }
}