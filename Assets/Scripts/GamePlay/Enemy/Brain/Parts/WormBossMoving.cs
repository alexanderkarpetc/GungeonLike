using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GamePlay.Enemy.Brain.Parts
{
  public class WormBossMoving : BotPart
  {
    private List<Vector2> targets;
    private int index = 0; 

    public WormBossMoving(BotBrain brain) : base(brain)
    {
      targets = FindPoints();
    }

    private List<Vector2> FindPoints()
    {
      return AppModel.CurrentRoom.GetRoomData().controlPoints.Select(x => (Vector2) x.position).ToList();
    }

    protected override void OnUpdate()
    {
      var agent = Brain.EnemyController.GetAiPath();
      if (targets.Count == 0) return;
      bool search = false;

      if (agent.reachedEndOfPath && !agent.pathPending)
      {
        index = index + 1;
        search = true;
      }
      
      index = index % targets.Count;
      agent.destination = targets[index]; // not sure

      if (search)
        agent.SearchPath();
    }
  }
}