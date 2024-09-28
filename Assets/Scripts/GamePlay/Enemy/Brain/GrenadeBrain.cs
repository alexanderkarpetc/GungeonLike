using GamePlay.Common;
using GamePlay.Enemy.Brain.Parts;
using UnityEngine;

namespace GamePlay.Enemy.Brain
{
  public class GrenadeBrain : BotBrain
  {
    public override void Init()
    {
      base.Init();
      _parts.Clear();
      var grenadeBotPart = new GrenadeBotPart(this);
      _parts.Add(grenadeBotPart);
      EnemyController.GetAiPath().maxSpeed = StaticData.GrenadeManSpeed;
      EnemyController.GetDestinationSetter().target = AppModel.PlayerTransform();
    }
  }
}