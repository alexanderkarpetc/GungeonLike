using GamePlay.Common;
using GamePlay.Enemy.Brain.Parts;
using UnityEngine;

namespace GamePlay.Enemy.Brain
{
  public class WormBossBrain : BotBrain
  {
    public override void Init()
    {
      base.Init();
      _parts.Clear();
      var wormBossMoving = new WormBossMoving(this);
      _parts.Add(wormBossMoving);
      EnemyController.GetAiPath().maxSpeed = StaticData.WormBossSpeedBase;
      EnemyController.SetHealthServerRpc(200);
    }
  }
}