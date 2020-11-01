using System.Collections;
using UnityEngine;

namespace GamePlay.Weapons
{
  public class Weapon : MonoBehaviour
  {
    public bool IsInverted;
    [HideInInspector] public bool IsPlayers;

    [SerializeField] protected GameObject _projectile;
    [SerializeField] protected Transform _shootPoint;
    [SerializeField] protected float _damage;
    [SerializeField] protected float _shootRate;
    [SerializeField] protected float _shootDelay;
    [SerializeField] protected float _bulletSpeed;
    [SerializeField] protected float _impulse;
    [SerializeField] protected int _magazineSize;
    [SerializeField] protected Animator _animator;

    protected int bulletsLeft;
    protected float _nextShotTime;
    protected bool _reloading;
    private static readonly int Reload = Animator.StringToHash("Reload");
    private static readonly int Shoot = Animator.StringToHash("Shoot");

    private void Start()
    {
      bulletsLeft = _magazineSize;
    }

    public void TryShoot()
    {
      if (bulletsLeft <= 0)
      {
        StartCoroutine(DoReload());

        return;
      }

      if (!(Time.time >= _nextShotTime) || _reloading)
        return;

      StartCoroutine(ShootCoroutine());
    }

    private IEnumerator ShootCoroutine()
    {
      _nextShotTime = Time.time + _shootRate;
      _animator.SetTrigger(Shoot);
      yield return new WaitForSeconds(_shootDelay);
      SpawnProjectiles();
      bulletsLeft--;
    }

    protected virtual void SpawnProjectiles()
    {
      var go = Instantiate(_projectile, _shootPoint.position, transform.rotation);
      var projectile = go.GetComponent<Projectile>();
      projectile.IsInverted = IsInverted;
      projectile.IsPlayerBullet = IsPlayers;
      projectile.Damage = _damage;
      projectile.Speed = _bulletSpeed;
      projectile.Impulse = _impulse;
    }

    private IEnumerator DoReload()
    {
      _reloading = true;
      _animator.SetTrigger(Reload);
      yield return new WaitForSeconds(_animator.GetCurrentAnimatorClipInfo(0).Length);
      bulletsLeft = _magazineSize;
      _reloading = false;
    }
  }
}