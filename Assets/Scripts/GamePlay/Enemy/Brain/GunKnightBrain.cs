using GamePlay.Common;
using GamePlay.Enemy.Brain.Parts;
using UnityEngine;

namespace GamePlay.Enemy.Brain
{
  public class GunKnightBrain : BotBrain
  {
    public GunKnightBrain(GameObject owner) : base(owner)
    {
      _parts.Clear();
      Owner = owner;
      var moving = new TargetFinder(this);
      var attacking = new GunKnightAttacking(this);
      _parts.Add(moving);
      _parts.Add(attacking);
    }
    public override void OnCreate()
    {
      EnemyController.GetAiPath().maxSpeed = StaticData.EnemyKnightSpeedBase;
      EnemyController.GetDestinationSetter().target = AppModel.PlayerTransform();
      EnemyController.State.Hp = 70;
    }
  }
}