using System;
using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
  public bool IsInverted;
  [HideInInspector] public bool IsPlayers;

  [SerializeField] private GameObject _projectile;
  [SerializeField] private Transform _shootPoint;
  [SerializeField] private float _damage;
  [SerializeField] private float _shootRate;
  [SerializeField] private int _magazineSize;
  [SerializeField] private Animator _animator;
  
  private int bulletsLeft;
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
      
    _animator.SetTrigger(Shoot);
    var go = Instantiate(_projectile, _shootPoint.position, transform.rotation);
    var projectile = go.GetComponent<Projectile>();
    projectile.IsInverted = IsInverted;
    projectile.IsPlayerBullet = IsPlayers;
    _nextShotTime = Time.time + _shootRate;
    bulletsLeft--;
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