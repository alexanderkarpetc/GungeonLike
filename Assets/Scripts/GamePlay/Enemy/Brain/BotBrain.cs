using System.Collections.Generic;
using GamePlay.Common;
using GamePlay.Enemy.Brain.Parts;
using UnityEngine;

namespace GamePlay.Enemy.Brain
{
  public class BotBrain : IUpdatable
  {
    public GameObject Target;
    public GameObject Owner;
    public EnemyController EnemyController;

    protected List<BotPart> _parts = new List<BotPart>();

    public BotBrain(GameObject owner)
    {
      Owner = owner;
      var shooting = new BulletBotShooting(this);
      var targetFinder = new TargetFinder(this);
      _parts.Add(shooting);
      _parts.Add(targetFinder);
      EnemyController = owner.GetComponent<EnemyController>();
    }

    public virtual void OnUpdate()
    {
      _parts.ForEach(x=> x.OnUpdate());
    }

    public virtual void OnCreate()
    {
      EnemyController.GetDestinationSetter().target = AppModel.PlayerTransform();
      EnemyController.GetAiPath().maxSpeed = StaticData.EnemyBulletSpeedBase;
    }
  }
}