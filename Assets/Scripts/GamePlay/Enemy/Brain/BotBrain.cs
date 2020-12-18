using System.Collections.Generic;
using GamePlay.Common;
using UnityEngine;

namespace GamePlay.Enemy.Brain
{
  public class BotBrain : IUpdatable
  {
    public GameObject Target;
    public GameObject Owner;
    public EnemyController _enemyController;

    protected List<BotPart> _parts = new List<BotPart>();
    protected BulletBotShooting _shooting;
    protected TargetFinder _targetFinder;

    public BotBrain(GameObject owner)
    {
      Owner = owner;
      _shooting = new BulletBotShooting(this);
      _targetFinder = new TargetFinder(this);
      _parts.Add(_shooting);
      _parts.Add(_targetFinder);
      _enemyController = owner.GetComponent<EnemyController>();
    }

    public virtual void OnUpdate()
    {
      _parts.ForEach(x=> x.OnUpdate());
    }

    public virtual void OnCreate()
    {
      _enemyController.GetDestinationSetter().target = AppModel.PlayerTransform();
      _enemyController.GetAiPath().maxSpeed = StaticData.EnemyBulletSpeedBase;
    }
  }
}