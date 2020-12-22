using System;
using GamePlay.Common;
using GamePlay.Weapons;
using UnityEngine;

namespace GamePlay.Level
{
  public class Turret : MonoBehaviour
  {
    [SerializeField] private GameObject _projectile;
    [SerializeField] private Transform _shootPoint;
    private static readonly int IsFiring = Animator.StringToHash("IsFiring");

    private void Start()
    {
      GetComponent<Animator>().SetBool(IsFiring, true);
    }

    // Invoked from animation
    public void Shoot()
    {
      var go = Instantiate(_projectile, _shootPoint.position, Quaternion.identity);
      go.transform.SetParent(AppModel.BulletContainer().transform);
      var projectile = go.GetComponent<Projectile>();
      projectile.Speed = WeaponStaticData.TurretBulletSpeed;
      projectile.Direction = Weapon.DegreeToVector2(transform.rotation.eulerAngles.z);
      projectile.Owner = gameObject;
    }
  }
}