using System.Linq;
using GamePlay.Common;
using GamePlay.Player;
using UnityEngine;

namespace GamePlay.Weapons
{
  public class Crossbow : Weapon
  {
    [SerializeField] private GameObject _laserSight;
    private bool _hasSightSkill;
    
    protected override void Start()
    {
      base.Start();
      CheckSkillExists();
      AppModel.PlayerState().OnSkillLearned += CheckSkillExists;
    }

    private void OnDestroy()
    {
      AppModel.PlayerState().OnSkillLearned -= CheckSkillExists;
    }

    private void CheckSkillExists()
    {
      _hasSightSkill = AppModel.PlayerState().Skills.Exists(x => x.Kind == SkillKind.Gunslinger);
    }

    protected override void InitProjectiles()
    {
      var go = BulletPoolManager.Instance.GetBulletFromPool(_projectile, _shootPoint.position, Quaternion.identity, projectileName);
      go.transform.SetParent(AppModel.BulletContainer().transform);
      var projectile = go.GetComponent<Projectile>();
      projectile.IsPlayerBullet = IsPlayers;
      projectile.Damage = BaseDamage;
      projectile.Speed = _hasSightSkill ? _bulletSpeed * 2 : _bulletSpeed;
      projectile.Impulse = _hasSightSkill ? _impulse * 2 : _impulse;

      projectile.Direction = Vector2.right;
      projectile.transform.rotation = Quaternion.Euler(0,0, transform.rotation.eulerAngles.z);
      if (IsInverted)
        projectile.Direction *= -1;
    }

    private void Update()
    {
      _laserSight.SetActive(_hasSightSkill && !reloading);

      if (_hasSightSkill && !reloading)
      {
        var direction = IsInverted
          ? DegreeToVector2(transform.rotation.eulerAngles.z) * new Vector2(-1, -1)
          : DegreeToVector2(transform.rotation.eulerAngles.z);
        var hit = Physics2D.RaycastAll(transform.position, direction)
          .First(x => !x.collider.CompareTag("Player") && 
                      !x.collider.CompareTag("Projectile") &&
                      !x.collider.CompareTag("Finish"));
        var length = Vector2.Distance(hit.point, _shootPoint.position);
        _laserSight.transform.localScale = new Vector3(1, length*3, 1);
      }
    }
  }
}