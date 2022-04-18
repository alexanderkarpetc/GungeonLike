using System;
using System.Collections.Generic;
using GamePlay.Common;
using GamePlay.Enemy;
using GamePlay.Player;
using GamePlay.Weapons;
using UnityEngine;

namespace GamePlay
{
  public class Projectile : MonoBehaviour
  {
    public GameObject Owner;
    private static List<string> _envTags = new List<string> {"Obstacle", "Environment"};
    [SerializeField] private GameObject _enemyHitFx;
    [SerializeField] private GameObject _envHitFx;
    [HideInInspector] public bool IsPlayerBullet;
    [HideInInspector] public float Speed;
    [HideInInspector] public float Impulse;
    [HideInInspector] public Vector2 Direction;
    [HideInInspector] public Weapon Weapon;

    void Update()
    {
      transform.Translate(Time.deltaTime * Speed * Direction);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
      // Hit self
      if(other.gameObject == Owner)
        return;
      // Hit ENV
      if (_envTags.Contains(other.tag))
      {
        var fx = Instantiate(_envHitFx, transform.position,
          Quaternion.LookRotation(Vector3.forward, Direction*new Vector2(-1,-1)));
        fx.transform.SetParent(AppModel.FxContainer().transform);
        if (other.CompareTag("Environment"))
        {
          DamageManager.Hit(other.GetComponent<Level.Environment>(), Weapon);
        }
        Destroy(gameObject);
      }
      if (!IsPlayerBullet && other.CompareTag("Player"))
      {
        HitPlayer(other);
        Destroy(gameObject);
      }
      
      if (IsPlayerBullet && other.CompareTag("Enemy"))
      {
        if (_enemyHitFx != null)
          Instantiate(_enemyHitFx, transform.position, transform.rotation);
        HitEnemy(other);
        Destroy(gameObject);
      }
    }

    private void HitPlayer(Collider2D other)
    {
      DamageManager.HitPlayer(other.GetComponent<PlayerController>());
    }

    private void HitEnemy(Collider2D enemy)
    {
      var enemyController = enemy.GetComponent<EnemyController>();
      DamageManager.Hit(enemyController, Weapon, transform.rotation * Direction.normalized * Impulse);
    }
  }
}