using System.Linq;
using UnityEngine;

namespace GamePlay.Enemy.Brain.Parts
{
  public class CubulonBotShooting : BotPart
  {
    private float _nextShotTime;
    private float _delay;

    public CubulonBotShooting(BotBrain brain) : base(brain)
    {
      _delay = brain.EnemyController.GetComponent<Animator>().runtimeAnimatorController.animationClips.ToList()
        .Find(x => x.name.Equals("DownLeft")).length;
      _nextShotTime = Time.time + _delay;

    }

    protected override void OnUpdate()
    {
      if (Time.time > _nextShotTime)
      {
        _nextShotTime = Time.time + _delay;
        Brain.ShootWeaponServerRpc();
      }
    }
  }
}