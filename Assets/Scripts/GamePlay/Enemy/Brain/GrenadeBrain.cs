using GamePlay.Common;
using GamePlay.Enemy.Brain.Parts;
using UnityEngine;

namespace GamePlay.Enemy.Brain
{
  public class GrenadeBrain : BotBrain
  {
    public GrenadeBrain(GameObject owner) : base(owner)
    {
      _parts.Clear();
      var grenadeBotPart = new GrenadeBotPart(this);
      _parts.Add(grenadeBotPart);
    }

    public override void OnCreate()
    {
      EnemyController.GetAiPath().maxSpeed = StaticData.GrenadeManSpeed;
      EnemyController.GetDestinationSetter().target = AppModel.PlayerTransform();
    }
  }
}