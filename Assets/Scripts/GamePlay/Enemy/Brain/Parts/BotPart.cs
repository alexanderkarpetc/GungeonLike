using UnityEngine;

namespace GamePlay.Enemy.Brain.Parts
{
  public abstract class BotPart : IUpdatable
  {
    protected BotBrain Brain;
    protected GameObject Owner => Brain.Owner;

    protected BotPart(BotBrain brain)
    {
      Brain = brain;
    }

    public abstract void OnUpdate();
  }
}