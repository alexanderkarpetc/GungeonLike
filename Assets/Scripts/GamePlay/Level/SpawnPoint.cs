using System;
using UnityEngine;

namespace GamePlay.Level
{
    public class SpawnPoint : MonoBehaviour
    {
        public GameObject SpawnObject;

        private void Start()
        {
            Invoke(nameof(Spawn), 0.5f);
        }

        public void Spawn()
        {
            Instantiate(SpawnObject, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
