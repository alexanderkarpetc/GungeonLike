using System.Collections.Generic;
using UnityEngine;

namespace GamePlay.Enemy.Brain
{
  public class CubulonBotMoving : BotPart
  {
    private List<Vector2> targets;
    private int index = 0; 

    public CubulonBotMoving(BotBrain brain) : base(brain)
    {
      targets = FindPoints();
    }

    private List<Vector2> FindPoints()
    {
      var roof = GameObject.Find("Roof").GetComponent<Collider2D>();
      return new List<Vector2>
      {
        new Vector2(roof.bounds.center.x + roof.bounds.size.x / 3, roof.bounds.center.y + roof.bounds.size.y / 3),
        new Vector2(roof.bounds.center.x - roof.bounds.size.x / 3, roof.bounds.center.y - roof.bounds.size.y / 3),
        new Vector2(roof.bounds.center.x + roof.bounds.size.x / 3, roof.bounds.center.y - roof.bounds.size.y / 3),
        new Vector2(roof.bounds.center.x - roof.bounds.size.x / 3, roof.bounds.center.y + roof.bounds.size.y / 3)
      };
    }

    public override void OnUpdate()
    {
      var agent = Brain._enemyController.GetAiPath();
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