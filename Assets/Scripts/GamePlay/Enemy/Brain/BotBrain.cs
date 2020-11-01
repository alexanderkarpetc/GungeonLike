using UnityEngine;

namespace GamePlay.Enemy.Brain
{
  public class BotBrain : IUpdatable
  {
    public GameObject Target;
    public GameObject Owner;
    private BotShooting _shooting;
    private TargetFinder _targetFinder;

    public BotBrain(GameObject owner)
    {
      Owner = owner;
      _shooting = new BotShooting(this);
      _targetFinder = new TargetFinder(this);
    }

    public void OnUpdate()
    {
      _targetFinder.OnUpdate();
      _shooting.OnUpdate();
    }
  }
}