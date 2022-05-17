using System.Linq;
using GamePlay.Weapons;
using UnityEngine;

namespace GamePlay.Enemy.Brain.Parts
{
  public class BulletBotShooting : BotPart
  {
    private Weapon _weapon;
    private EnemyController _controller;
    public BulletBotShooting(BotBrain brain) : base(brain)
    {
      _controller = Owner.GetComponent<EnemyController>();
      _weapon = _controller.Weapon;
    }
    protected override void OnUpdate()
    {
      if(Brain.Target == null)
        return;
      var raycast = Physics2D.LinecastAll(Brain.Owner.transform.position, Brain.Target.transform.position)
        .Where(x=>x.collider.CompareTag("Obstacle") || x.collider.CompareTag("Player"));
      if (raycast.First().collider.CompareTag("Player"))
        TryShoot();
    }

    private void TryShoot()
    {
      _weapon.TryShoot();
    }
  }
}