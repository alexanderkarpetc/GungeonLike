using System;
using UnityEngine;

namespace GamePlay.Level
{
  public class Environment : MonoBehaviour
  {
    [SerializeField] private float Health;
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
      Destroy(gameObject);
    }
  }
}