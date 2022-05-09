using System.Linq;
using GamePlay.Common;
using GamePlay.Weapons;
using UnityEngine;

namespace GamePlay.Enemy.Brain.Parts
{
  public class SniperBotShooting : BotPart
  {
    private float _playerNoticeTime = 0.5f;
    private float _aimTime = 1f;
    private LaserWeapon _weapon;
    private EnemyController _controller;
    private float _enemySawTime;
    private bool _isAiming;

    public SniperBotShooting(BotBrain brain) : base(brain)
    {
      _controller = Owner.GetComponent<EnemyController>();
      _weapon = (LaserWeapon) _controller.Weapon;
    }
    public override void OnUpdate()
    {
      if(Brain.Target == null)
        return;
      var raycast = Physics2D.LinecastAll(Brain.Owner.transform.position, AppModel.PlayerGameObj().transform.position)
        .Where(x=>x.collider.CompareTag("Obstacle") || x.collider.CompareTag("Player"));
      if (!raycast.First().collider.CompareTag("Player"))
      {
        Brain.EnemyController.CurrentTarget = Brain.EnemyController.GetAiPath().destination;
        Debug.Log(Brain.EnemyController.CurrentTarget);
        StopAim();
        return;
      }
      _enemySawTime += Time.deltaTime;
      if (_enemySawTime > _playerNoticeTime && !_isAiming)
        StartAim();
      if (_enemySawTime > _aimTime + _playerNoticeTime)
        TryShoot();
    }

    private void StartAim()
    {
      Brain.EnemyController.GetAiPath().maxSpeed = 0;
      Brain.EnemyController.CurrentTarget = null;
      _isAiming = true;
      _weapon.StartAim();
    }

    private void StopAim()
    {
      Brain.EnemyController.GetAiPath().maxSpeed = StaticData.EnemySniperSpeedBase;
      _enemySawTime = 0;
      _isAiming = false;
      _weapon.StopAim();
    }

    private void TryShoot()
    {
      _enemySawTime = _playerNoticeTime;
      _weapon.TryShoot();
    }
  }
}