using System;
using UnityEngine;

namespace GamePlay.Level
{
  public class Environment : MonoBehaviour
  {
    [SerializeField] private float Health;
    [SerializeField] protected GameObject DestroyFx;
    public bool IsDestroying;

    public void DealDamage(float damage)
    {
      Health -= damage;
      if (Health <= 0)
      {
        IsDestroying = true;
        DoDestroy();
      }
    }

    protected virtual void DoDestroy()
    {
      if(DestroyFx != null)
        Instantiate(DestroyFx, transform.position, Quaternion.identity);
      Destroy(gameObject);
    }
  }
}