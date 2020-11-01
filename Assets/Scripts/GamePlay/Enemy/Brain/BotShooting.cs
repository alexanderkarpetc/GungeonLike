using System.Linq;
using GamePlay.Weapons;
using UnityEngine;

namespace GamePlay.Enemy.Brain
{
  public class BotShooting : BotPart
  {
    private Weapon _weapon;
    private EnemyController _controller;
    public BotShooting(BotBrain brain) : base(brain)
    {
      _controller = Owner.GetComponent<EnemyController>();
      _weapon = _controller.Weapon;
    }
    public override void OnUpdate()
    {
      var raycast = Physics2D.LinecastAll(Brain.Owner.transform.position, Brain.Target.transform.position)
        .Where(x=>!x.collider.CompareTag("Projectile") && !x.collider.CompareTag("Enemy"));
      if (raycast.First().collider.CompareTag("Player"))
        TryShoot();
    }

    private void TryShoot()
    {
      _weapon.TryShoot();
    }
  }
}