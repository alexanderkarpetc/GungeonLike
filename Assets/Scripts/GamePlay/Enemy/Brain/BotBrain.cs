using System;
using System.Collections.Generic;
using GamePlay.Common;
using GamePlay.Enemy.Brain.Parts;
using GamePlay.Weapons;
using Unity.Netcode;
using UnityEngine;

namespace GamePlay.Enemy.Brain
{
  public class BotBrain : NetworkBehaviour
  {
    public GameObject Target;

    [HideInInspector] public EnemyController EnemyController;

    protected List<BotPart> _parts = new();

    public virtual void Init()
    {
      var shooting = new BulletBotShooting(this);
      var targetFinder = new TargetFinder(this);
      _parts.Add(shooting);
      _parts.Add(targetFinder);
      EnemyController = GetComponent<EnemyController>();
      EnemyController.GetDestinationSetter().target = AppModel.PlayerTransform();
      EnemyController.GetAiPath().maxSpeed = StaticData.EnemyBulletSpeedBase;
    }

    public void ClientInit()
    {
      EnemyController = GetComponent<EnemyController>();
    }

    public virtual void Update()
    {
      if(!IsServer) return;
      _parts.ForEach(x=> x.Update());
    }

    [ServerRpc]
    public void ShootWeaponServerRpc()
    {
      ShootClientRpc();
    }

    [ClientRpc]
    private void ShootClientRpc()
    {
      EnemyController.Weapon.TryShoot();
    }
  }
}