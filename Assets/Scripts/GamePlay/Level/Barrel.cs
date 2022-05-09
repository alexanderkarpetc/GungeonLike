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
      DamageManager.Explode(transform.position, Radius, Damage);
    }
  }
}
