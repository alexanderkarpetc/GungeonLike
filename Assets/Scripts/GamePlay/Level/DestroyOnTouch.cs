using System;
using UnityEngine;

namespace GamePlay.Level
{
    public class DestroyOnTouch : MonoBehaviour
    {
        private Environment _environment;
        private float _spawnTime;
        private const float _invincibleTime = 0.2f;

        private void Start()
        {
            _environment = GetComponent<Environment>();
            _spawnTime = Time.time;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if(col.CompareTag("Player") && _spawnTime + _invincibleTime > Time.time)
                return;
            if(col.CompareTag("Player") || col.CompareTag("Environment") || col.CompareTag("Obstacle"))
                _environment.DealDamage(100);
        }
    }
}