using UnityEngine;

namespace Enemy
{
  public class TargetFinder : BotPart
  {
    public TargetFinder(BotBrain brain) : base(brain)
    {
      brain.Target = GameObject.Find("Player");
    }

    public override void OnUpdate() { }
  }
}