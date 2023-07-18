using System.Collections.Generic;
using UnityEngine;

namespace GamePlay.Common
{
  public class BulletPoolManager : MonoBehaviour
  {
    public static BulletPoolManager Instance;

    // A dictionary to store pools for different bullet prefabs
    private Dictionary<string, Queue<Projectile>> bulletPools = new();

    private void Awake()
    {
      if (Instance == null)
        Instance = this;
    }

    private Queue<Projectile> GetOrCreatePool(string projectileName)
    {
      if (!bulletPools.ContainsKey(projectileName))
        bulletPools[projectileName] = new Queue<Projectile>();

      return bulletPools[projectileName];
    }

    public Projectile GetBulletFromPool(GameObject bulletPrefab, Vector3 position, Quaternion rotation, string projectileName)
    {
      var bulletPool = GetOrCreatePool(projectileName);

      if (bulletPool.Count == 0)
      {
        // If the pool is empty, create a new bullet and add it to the pool
        var newBullet = Instantiate(bulletPrefab, position, rotation).GetComponent<Projectile>();
        return newBullet;
      }

      // Dequeue a bullet from the pool and set its position and rotation
      var bullet = bulletPool.Dequeue();
      bullet.transform.position = position;
      bullet.transform.rotation = rotation;
      bullet.gameObject.SetActive(true);

      return bullet;
    }

    public void ReturnBulletToPool(Projectile bullet, string projectileName)
    {
      bullet.gameObject.SetActive(false);
      bullet.CleanUp();

      // Find the corresponding pool for the bullet's prefab and enqueue the bullet
      foreach (var pool in bulletPools)
      {
        if (pool.Key.Equals(projectileName))
        {
          pool.Value.Enqueue(bullet);
          return;
        }
      }
    }
  }
}