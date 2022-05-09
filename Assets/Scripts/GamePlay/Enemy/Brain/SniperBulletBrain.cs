using GamePlay.Common;
using GamePlay.Enemy.Brain.Parts;
using UnityEngine;

namespace GamePlay.Enemy.Brain
{
  public class SniperBulletBrain : BotBrain
  {

    public SniperBulletBrain(GameObject owner) : base(owner)
    {
      _parts.Clear();
      Owner = owner;
      var shooting = new SniperBotShooting(this);
      var moving = new SmallPatrolBotMoving(this);
      _parts.Add(shooting);
      _parts.Add(moving);
      EnemyController = owner.GetComponent<EnemyController>();
    }
    public override void OnCreate()
    {
      EnemyController.GetAiPath().maxSpeed = StaticData.EnemySniperSpeedBase;
    }
  }
}