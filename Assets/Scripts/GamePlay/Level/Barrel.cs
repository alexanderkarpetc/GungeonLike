using GamePlay.Common;
using GamePlay.Enemy;
using GamePlay.Player;
using UnityEngine;

namespace GamePlay.Level
{
  public class Barrel : Environment
  {
    [SerializeField] public float Damage;
    [SerializeField] private GameObject BoomFx;
    [SerializeField] private float Radius;
    protected override void DoDestroy()
    {
      Instantiate(BoomFx, transform.position, Quaternion.Euler(0,0,90));
      DamageSurroundings();
      Destroy(gameObject);
      base.DoDestroy();
    }

    private void DamageSurroundings()
    {
      var hits = Physics2D.OverlapCircleAll(transform.position, Radius);
      foreach (var hit in hits)
      {
        if (hit.CompareTag("Enemy"))
        {
          DamageManager.Hit(hit.GetComponent<EnemyController>(), this);
        }
        if (hit.CompareTag("Player"))
        {
          DamageManager.HitPlayer(hit.GetComponent<PlayerController>());
        }
        if (hit.CompareTag("Environment"))
        {
            var environment = hit.GetComponent<Environment>();
            if(!environment.IsDestroying)
              environment.DealDamage(Damage);

        }
      }
    }
  }
}
