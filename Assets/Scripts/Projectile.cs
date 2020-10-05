using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private float _speed;
    void Update()
    {
        transform.Translate(Time.deltaTime * _speed * Vector2.right);
    }
}
