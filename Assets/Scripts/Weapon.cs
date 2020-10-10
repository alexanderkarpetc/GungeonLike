using UnityEngine;

namespace Player
{
  public class Weapon : MonoBehaviour
  {
    public bool IsInverted;
    [HideInInspector] public bool IsPlayers;

    [SerializeField] private GameObject _projectile;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private float _damage;
    [SerializeField] private float _shootRate;
    [SerializeField] private float _reloadTime;


    protected float _nextShotTime;
    protected bool _reloading;

    // Update is called once per frame
    protected virtual void Update()
    {
    }

    public void TryShoot()
    {
      if (!(Time.time >= _nextShotTime) || _reloading) 
        return;
      
      var go = Instantiate(_projectile, _shootPoint.position, transform.rotation);
      var projectile = go.GetComponent<Projectile>();
      projectile.IsInverted = IsInverted;
      projectile.IsPlayerBullet = IsPlayers;
      _nextShotTime = Time.time + _shootRate;
    }
  }
}