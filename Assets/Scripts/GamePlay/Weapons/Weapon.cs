using System.Collections;
using System.Linq;
using UnityEngine;

namespace GamePlay.Weapons
{
  public class Weapon : MonoBehaviour
  {
    public bool IsDoubleHanded;
    public int MagazineSize;
    public Sprite _uiImage;
    [HideInInspector] public int bulletsLeft;
    [HideInInspector] public bool IsInverted;

    [HideInInspector] public bool IsPlayers;

    [SerializeField] protected GameObject _projectile;
    [SerializeField] protected Transform _shootPoint;
    [SerializeField] protected float _damage;
    [SerializeField] protected float _shootRate;
    [SerializeField] protected float _shootDelay;
    [SerializeField] protected float _bulletSpeed;
    [SerializeField] protected float _impulse;
    [SerializeField] protected Animator _animator;

    protected float _nextShotTime;
    public bool reloading;
    public float reloadingTime;
    private static readonly int ReloadAnim = Animator.StringToHash("Reload");
    private static readonly int ShootAnim = Animator.StringToHash("Shoot");

    private void Start()
    {
      bulletsLeft = MagazineSize;
      reloadingTime = _animator.runtimeAnimatorController.animationClips.First(x=>x.name.Equals("Reload")).averageDuration;
    }

    public void TryShoot()
    {
      if (bulletsLeft <= 0)
      {
        Reload();

        return;
      }

      if (!(Time.time >= _nextShotTime) || reloading)
        return;

      StartCoroutine(ShootCoroutine());
    }

    public void Reload()
    {
      StartCoroutine(DoReload());
    }

    private IEnumerator ShootCoroutine()
    {
      _nextShotTime = Time.time + _shootRate;
      _animator.SetTrigger(ShootAnim);
      yield return new WaitForSeconds(_shootDelay);
      SpawnProjectiles();
      bulletsLeft--;
    }

    protected virtual void SpawnProjectiles()
    {
      var go = Instantiate(_projectile, _shootPoint.position, Quaternion.identity);
      var projectile = go.GetComponent<Projectile>();
      projectile.IsPlayerBullet = IsPlayers;
      projectile.Damage = _damage;
      projectile.Speed = _bulletSpeed;
      projectile.Impulse = _impulse;

      projectile.Direction = DegreeToVector2(transform.rotation.eulerAngles.z);
      if (IsInverted)
        projectile.Direction *= -1;
    }

    private IEnumerator DoReload()
    {
      reloading = true;
      _animator.SetTrigger(ReloadAnim);
      yield return new WaitForSeconds(reloadingTime);
      bulletsLeft = MagazineSize;
      reloading = false;
    }
    public static Vector2 RadianToVector2(float radian)
    {
      return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }
  
    public static Vector2 DegreeToVector2(float degree)
    {
      return RadianToVector2(degree * Mathf.Deg2Rad);
    }
  }
}