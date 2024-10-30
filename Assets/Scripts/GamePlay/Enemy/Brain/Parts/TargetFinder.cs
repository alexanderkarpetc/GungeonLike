using System.Collections;
using UnityEngine;

namespace GamePlay.Enemy.Brain.Parts
{
  public class TargetFinder : BotPart
  {
    public TargetFinder(BotBrain brain) : base(brain) { }

    public override void Init()
    {
      Brain.StartCoroutine(ChangeTarget());
    }

    private IEnumerator ChangeTarget()
    {
      Brain.Target = AppModel.PlayerGameObj();
      while (true)
      {
        var players = AppModel.PlayerGameObjs();
        // find the closest player
        players.ForEach(player =>
        {
          if (Vector3.Distance(Brain.gameObject.transform.position, player.transform.position) <
              Vector3.Distance(Brain.gameObject.transform.position, Brain.Target.transform.position))
            Brain.Target = player;
          Brain.EnemyController.GetDestinationSetter().target = Brain.Target.transform;
        });
        yield return new WaitForSeconds(1);
      }
    }
  }
}