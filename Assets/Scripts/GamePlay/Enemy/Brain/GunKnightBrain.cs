using GamePlay.Common;
using GamePlay.Enemy.Brain.Parts;
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
      EnemyController.GetDestinationSetter().target = AppModel.PlayerTransform();
      EnemyController.SetHealthServerRpc(70);
    }
  }
}