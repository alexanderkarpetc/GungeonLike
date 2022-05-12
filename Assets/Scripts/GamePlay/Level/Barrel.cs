using GamePlay.Common;
using GamePlay.Enemy;
using GamePlay.Player;
using UnityEngine;

namespace GamePlay.Level
{
  public class Barrel : Environment
  {
    [SerializeField] public float Damage;
    [SerializeField] private float Radius;
    protected override void DoDestroy()
    {
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
