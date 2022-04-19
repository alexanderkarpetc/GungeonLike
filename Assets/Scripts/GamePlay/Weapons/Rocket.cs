using System;
using GamePlay.Common;
using UnityEngine;

namespace GamePlay.Weapons
{
    public class Rocket : Projectile
    {
        public float Radius;

        private void Start()
        {
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(Direction.y, Direction.x) * 180 / Mathf.PI);
            Direction = Vector2.right;
        }

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (_envTags.Contains(other.tag) || other.CompareTag("Player") || other.CompareTag("Enemy"))
            {
                Explode();
                var fx = Instantiate(_envHitFx, transform.position,
                    Quaternion.LookRotation(Vector3.forward, Direction*new Vector2(-1,-1)));
                fx.transform.SetParent(AppModel.FxContainer().transform);
                Destroy(gameObject);
            }
        }

        private void Explode()
        {
            var hits = Physics2D.OverlapCircleAll(transform.position, Radius);
            foreach (var hit in hits)
            {
                if (_envTags.Contains(hit.tag))
                {
                    if (hit.CompareTag("Environment"))
                    {
                        DamageManager.Hit(hit.GetComponent<Level.Environment>(), Weapon);
                    }
                }
                if(hit.CompareTag("Player"))
                    HitPlayer(hit);
                if(hit.CompareTag("Enemy"))
                    HitEnemy(hit);
            }
        }
    }
}