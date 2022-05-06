using GamePlay.Common;
using GamePlay.Enemy.Brain.Parts;
using UnityEngine;

namespace GamePlay.Enemy.Brain
{
  public class WormBossBrain : BotBrain
  {

    public WormBossBrain(GameObject owner) : base(owner)
    {
      _parts.Clear();
      var wormBossMoving = new WormBossMoving(this);
      _parts.Add(wormBossMoving);
    }

    public override void OnCreate()
    {
      EnemyController.GetAiPath().maxSpeed = StaticData.WormBossSpeedBase;
      EnemyController.State.Hp = 100;
    }
  }
}