using System;
using System.Collections;
using System.Collections.Generic;
using Enemy;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private static List<string> _envTags = new List<string> {"Obstacle", "Environment"};

    public float Impulse;

    [SerializeField] private float _speed;
    [SerializeField] private GameObject _enemyHitFx;
    [SerializeField] private GameObject _envHitFx;
    public bool IsInverted;
    public bool IsPlayerBullet;
    public float Damage;

    void Update()
    {
        transform.Translate(Time.deltaTime * _speed * (IsInverted ? Vector2.left : Vector2.right));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Hit ENV
        if (_envTags.Contains(collision.collider.tag))
        {
            Instantiate(_envHitFx, transform.position, Quaternion.LookRotation (Vector3.forward, collision.GetContact(0).normal));
            Destroy(gameObject);
        }

        if (IsPlayerBullet && collision.collider.CompareTag("Enemy"))
        {
            Instantiate(_enemyHitFx, transform.position, transform.rotation);
            HitEnemy(collision.collider);
            Destroy(gameObject);
        }
        
        if (collision.collider.CompareTag("Player"))
        {
            HitPlayer(collision.collider);
            Destroy(gameObject);
        }
    }

    private void HitPlayer(Collider2D player)
    {
        
    }

    private void HitEnemy(Collider2D enemy)
    {
        var enemyController = enemy.GetComponent<EnemyController>();
        enemyController.Hit(Damage, transform.rotation* (IsInverted ? Vector2.left : Vector2.right) * Impulse);
    }
}
