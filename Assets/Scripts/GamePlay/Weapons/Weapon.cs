using System.Collections;
using System.Linq;
using GamePlay.Common;
using GamePlay.Player;
using UnityEngine;

namespace GamePlay.Weapons
{
  public class Weapon : MonoBehaviour
  {
    public bool IsDoubleHanded;
    public int MagazineSize;
    public Sprite _uiImage;
    [HideInInspector] public bool IsInverted;

    public bool IsPlayers;
    public WeaponType Type;
    public AmmoKind AmmoKind;
    [HideInInspector] public WeaponState State;

    [SerializeField] protected GameObject _projectile;
    [SerializeField] protected Transform _shootPoint;
    [SerializeField] protected float _shootRate;
    [SerializeField] protected float _shootDelay;
    [SerializeField] protected float _bulletSpeed;
    [SerializeField] protected float _impulse;
    [SerializeField] protected Animator _animator;

    protected float _nextShotTime;
    [HideInInspector] public bool reloading;
    [HideInInspector] public float reloadingTime;
    [HideInInspector] public float BaseDamage;
    private static readonly int ReloadAnim = Animator.StringToHash("Reload");
    private static readonly int ShootAnim = Animator.StringToHash("Shoot");

    protected virtual void Start()
    {
      if (!IsPlayers)
      {
        State = new WeaponState{bulletsLeft = Mathf.Min(MagazineSize, AppModel.Player().Backpack.Ammo[AmmoKind])};
      }
      reloadingTime = _animator.runtimeAnimatorController.animationClips.First(x=>x.name.Equals("Reload")).averageDuration;
      BaseDamage  = WeaponStaticData.WeaponDamage[Type];
    }

    public virtual void TryShoot()
    {
      if (State.bulletsLeft <= 0)
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
      if (IsPlayers && AppModel.Player().Backpack.Ammo[AmmoKind] == 0)
        return;
      StartCoroutine(DoReload());
    }

    private IEnumerator ShootCoroutine()
    {
      if (IsPlayers)
        AppModel.Player().Backpack.Ammo[AmmoKind]--;
      _nextShotTime = Time.time + _shootRate;
      _animator.SetTrigger(ShootAnim);
      yield return new WaitForSeconds(_shootDelay);
      SpawnProjectiles();
      State.bulletsLeft--;
      if (State.bulletsLeft <= 0)
      {
        Reload();
      }
    }

    protected virtual void SpawnProjectiles()
    {
      var go = Instantiate(_projectile, _shootPoint.position, Quaternion.identity);
      go.transform.SetParent(AppModel.BulletContainer().transform);
      var projectile = go.GetComponent<Projectile>();
      projectile.IsPlayerBullet = IsPlayers;
      projectile.Weapon = this;
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
      if(IsPlayers)
        State.bulletsLeft = Mathf.Min(MagazineSize, AppModel.Player().Backpack.Ammo[AmmoKind]);
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

    public int GetPower()
    {
      return (int)(WeaponStaticData.WeaponDamage[Type] / _shootRate + MagazineSize / reloadingTime);
    }
  }
}