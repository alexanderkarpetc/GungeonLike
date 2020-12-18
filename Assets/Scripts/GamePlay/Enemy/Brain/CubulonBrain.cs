using GamePlay.Common;
using UnityEngine;

namespace GamePlay.Enemy.Brain
{
  public class CubulonBrain : BotBrain
  {

    public CubulonBrain(GameObject owner) : base(owner)
    {
      _parts.Clear();
      var cubulonShooting = new CubulonBotShooting(this);
      var cubulonMoving = new CubulonBotMoving(this);
      _parts.Add(cubulonShooting);
      _parts.Add(cubulonMoving);
    }

    public override void OnCreate()
    {
      _enemyController.GetAiPath().maxSpeed = StaticData.EnemyCubulonSpeedBase;
    }
  }
}