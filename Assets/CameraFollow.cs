using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform _player;
    public float Speed;

    void Start()
    {
        _player = GameObject.Find("Player").transform;
    }

    void Update()
    {
        if(_player != null)
        {
            var targetPos = new Vector3(_player.position.x, _player.position.y, -10);
            transform.position = Vector3.Lerp(transform.position, targetPos, Speed);
        }
    }
}
