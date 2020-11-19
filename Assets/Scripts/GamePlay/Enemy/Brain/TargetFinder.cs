using UnityEngine;

namespace GamePlay.Enemy.Brain
{
  public class TargetFinder : BotPart
  {
    public TargetFinder(BotBrain brain) : base(brain)
    {
      brain.Target = AppModel.PlayerGameObj();
    }

    public override void OnUpdate() { }
  }
}