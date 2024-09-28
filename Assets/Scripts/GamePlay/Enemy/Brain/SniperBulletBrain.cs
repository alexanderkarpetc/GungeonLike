using GamePlay.Common;
using GamePlay.Enemy.Brain.Parts;
using UnityEngine;

namespace GamePlay.Enemy.Brain
{
  public class SniperBulletBrain : BotBrain
  {
    public override void Init()
    {
      base.Init();
      _parts.Clear();
      var shooting = new SniperBotShooting(this);
      var moving = new SmallPatrolBotMoving(this);
      _parts.Add(shooting);
      _parts.Add(moving);
      EnemyController.GetAiPath().maxSpeed = StaticData.EnemySniperSpeedBase;
    }
  }
}