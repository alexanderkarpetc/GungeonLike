using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private float _speed;
    public bool IsInverted;
    void Update()
    {
        transform.Translate(Time.deltaTime * _speed * (IsInverted ? Vector2.left : Vector2.right));
    }
}
