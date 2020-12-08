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
      var hits = Physics2D.CircleCastAll(transform.position, Radius, Vector2.right);
      foreach (var hit in hits)
      {
        if (hit.collider.CompareTag("Enemy"))
        {
          hit.collider.GetComponent<EnemyController>().Hit(Damage, Vector2.zero);
        }
        if (hit.collider.CompareTag("Player"))
        {
          hit.collider.GetComponent<PlayerController>().Hit();
        }
        if (hit.collider.CompareTag("Environment"))
        {
            var environment = hit.collider.GetComponent<Environment>();
            if(!environment.IsDestroying)
              environment.DealDamage(Damage);

        }
      }
    }
  }
}
