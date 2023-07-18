using GamePlay.Common;
using GamePlay.Weapons;
using UnityEngine;

namespace GamePlay.Level
{
  public class Turret : Environment
  {
    [SerializeField] private GameObject _projectile;
    [SerializeField] private Transform _shootPoint;
    private static readonly int IsFiring = Animator.StringToHash("IsFiring");
    private string _projectileName;

    private void Start()
    {
      GetComponent<Animator>().SetBool(IsFiring, true);
      _projectileName = _projectile.GetComponent<Projectile>().ProjectileName;
    }

    // Invoked from animation
    public void Shoot()
    {
      var go = BulletPoolManager.Instance.GetBulletFromPool(_projectile, _shootPoint.position, Quaternion.identity, _projectileName);
      go.transform.SetParent(AppModel.BulletContainer().transform);
      var projectile = go.GetComponent<Projectile>();
      projectile.Speed = AppModel.WeaponData().TurretBulletSpeed;
      projectile.Direction = Weapon.DegreeToVector2(transform.rotation.eulerAngles.z);
      projectile.Owner = gameObject;
    }
  }
}