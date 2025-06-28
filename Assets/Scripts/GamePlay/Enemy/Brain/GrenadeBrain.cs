using GamePlay.Common;
using GamePlay.Enemy.Brain.Parts;
using Unity.Netcode;
using UnityEngine;

namespace GamePlay.Enemy.Brain
{
  public class GrenadeBrain : BotBrain
  {
    public override void Init()
    {
      base.Init();
      _parts.Clear();
      var grenadeBotPart = new GrenadeBotPart(this);
      _parts.Add(grenadeBotPart);
      EnemyController.GetAiPath().maxSpeed = StaticData.GrenadeManSpeed;
      // EnemyController.GetDestinationSetter().target = AppModel.PlayerTransform();
    }
    
    [ServerRpc]
    public void ExplodeServerRpc()
    {
      ExplodeClientRpc();
    }

    [ClientRpc]
    private void ExplodeClientRpc()
    {
      ExplodeFx();
    }
            
    private void ExplodeFx()
    {
      // todo move it loader
      var boom = Resources.Load<GameObject>("Vfx/Explosion/BoomFx");

      var transformPosition = transform.position;
      DamageManager.Explode(transformPosition, 2, 50);
      Instantiate(boom, transformPosition, Quaternion.identity);
    }
  }
}