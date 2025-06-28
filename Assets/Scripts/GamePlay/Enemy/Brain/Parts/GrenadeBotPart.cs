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

        protected override void OnUpdate()
        {
            var distance = Vector3.Distance(Brain.gameObject.transform.position, Brain.Target.transform.position);
            if (distance < 1)
                Explode(Brain.EnemyController);
        }

        private void Explode(EnemyController controller)
        {
            ((GrenadeBrain)Brain).ExplodeServerRpc();
        }

    }
}