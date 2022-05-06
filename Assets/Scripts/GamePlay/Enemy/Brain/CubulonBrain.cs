using GamePlay.Common;
using GamePlay.Enemy.Brain.Parts;
using UnityEngine;

namespace GamePlay.Enemy.Brain
{
  public class CubulonBrain : BotBrain
  {

    public CubulonBrain(GameObject owner) : base(owner)
    {
      _parts.Clear();
      var cubulonShooting = new CubulonBotShooting(this);
      var cubulonMoving = new PatrolBotMoving(this);
      _parts.Add(cubulonShooting);
      _parts.Add(cubulonMoving);
    }

    public override void OnCreate()
    {
      EnemyController.GetAiPath().maxSpeed = StaticData.EnemyCubulonSpeedBase;
    }
  }
}