using GamePlay.Enemy;
using GamePlay.Player;
using UnityEngine;

namespace GamePlay.Level
{
  public class Barrel : Environment
  {
    [SerializeField] private GameObject BoomFx;
    [SerializeField] private float Radius;
    [SerializeField] private float Damage;
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
          hit.GetComponent<EnemyController>().Hit(Damage, Vector2.zero);
        }
        if (hit.CompareTag("Player"))
        {
          hit.GetComponent<PlayerController>().Hit();
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
