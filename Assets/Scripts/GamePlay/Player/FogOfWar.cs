using System;
using System.Collections;
using UnityEngine;

namespace GamePlay.Player
{
    public class FogOfWar: MonoBehaviour
    {
        private GameObject _fog;
        private Material _fogMat;
        private void Start()
        {
            var prefab = Resources.Load<GameObject>("Prefabs/FogOfWar");
            _fog = Instantiate(prefab, Camera.main.gameObject.transform);
            _fogMat = _fog.GetComponent<SpriteRenderer>().material;
            StartCoroutine(DismissFog());
        }

        private IEnumerator DismissFog()
        {
            var currentVal = 0f;

            while (true)
            {
                currentVal += Time.deltaTime * 0.25f;
                _fogMat.SetFloat("_Vision", currentVal);
                yield return null;
                if(currentVal >= 0.5f)
                    break;
            }
            Destroy(_fog);
        }
    }
}