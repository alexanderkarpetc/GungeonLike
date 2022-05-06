using System.Collections.Generic;
using UnityEngine;

namespace GamePlay.Enemy.Brain.Parts
{
    public class SmallPatrolBotMoving : PatrolBotMoving
    {
        public SmallPatrolBotMoving(BotBrain brain) : base(brain) { }
        protected override List<Vector2> FindPoints()
        {
            var start = Owner.transform.position;
            return new List<Vector2>
            {
                new Vector2(start.x + 3, start.y + 3),
                new Vector2(start.x - 3, start.y + 3),
                new Vector2(start.x - 3, start.y - 3),
                new Vector2(start.x + 3, start.y - 3)
            };
        }
    }
}