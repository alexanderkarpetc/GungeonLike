using System;
using UnityEngine;

namespace GamePlay.Level
{
  public class Environment : MonoBehaviour
  {
    [SerializeField] private float Health;
    [SerializeField] protected GameObject DestroyFx;
    
    private bool _isDestroying;

    public void DealDamage(float damage)
    {
      if(_isDestroying)
        return;
      Health -= damage;
      if (Health <= 0)
      {
        _isDestroying = true;
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