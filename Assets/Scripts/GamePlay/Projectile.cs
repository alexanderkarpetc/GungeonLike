﻿using System;
using System.Collections.Generic;
using GamePlay.Enemy;
using GamePlay.Player;
using UnityEngine;

namespace GamePlay
{
  public class Projectile : MonoBehaviour
  {
    private static List<string> _envTags = new List<string> {"Obstacle", "Environment"};
    [SerializeField] private GameObject _enemyHitFx;
    [SerializeField] private GameObject _envHitFx;
    [HideInInspector] public bool IsPlayerBullet;
    [HideInInspector] public float Damage;
    [HideInInspector] public float Speed;
    [HideInInspector] public float Impulse;
    [HideInInspector] public Vector2 Direction;

    void Update()
    {
      transform.Translate(Time.deltaTime * Speed * Direction);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
      // Hit ENV
      if (_envTags.Contains(collision.collider.tag))
      {
        var fx = Instantiate(_envHitFx, transform.position,
          Quaternion.LookRotation(Vector3.forward, collision.GetContact(0).normal));
        fx.transform.SetParent(AppModel.FxContainer().transform);
        if (collision.collider.CompareTag("Environment"))
        {
          collision.collider.GetComponent<Level.Environment>().DealDamage(Damage);
        }
        Destroy(gameObject);
      }

      if (!IsPlayerBullet && collision.collider.CompareTag("Player"))
      {
        HitPlayer(collision.collider);
        Destroy(gameObject);
      }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
      if (IsPlayerBullet && other.CompareTag("Enemy"))
      {
        Instantiate(_enemyHitFx, transform.position, transform.rotation);
        HitEnemy(other);
        Destroy(gameObject);
      }
    }

    private void HitPlayer(Collider2D player)
    {
      var playerController = player.GetComponent<PlayerController>();
      playerController.Hit();
    }

    private void HitEnemy(Collider2D enemy)
    {
      var enemyController = enemy.GetComponent<EnemyController>();
      enemyController.Hit(Damage, transform.rotation * Direction.normalized * Impulse);
    }
  }
}