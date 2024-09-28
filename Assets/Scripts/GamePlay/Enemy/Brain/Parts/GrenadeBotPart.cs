using GamePlay.Common;
using UnityEngine;

namespace GamePlay.Enemy.Brain.Parts
{
  public class GrenadeBotPart : BotPart
  {
    public GrenadeBotPart(BotBrain brain) : base(brain)
    {
      brain.Target = AppModel.PlayerGameObj();
      Brain.EnemyController.OnDeath += Explode;
    }

    public void Explode(EnemyController controller)
    {
      var boom = Resources.Load<GameObject>("Vfx/Explosion/BoomFx");
      
      DamageManager.Explode(Brain.Owner.transform.position, 2, 50);
      GameObject.Instantiate(boom, Brain.Owner.transform.position, Quaternion.identity);
    }

    protected override void OnUpdate()
    {
      var distance = Vector3.Distance(Brain.Owner.transform.position, Brain.Target.transform.position);
      if(distance < 1)
        DamageManager.Hit(Brain.EnemyController, Brain.EnemyController.State.Value.Hp, Vector2.zero);
    }
  }
}