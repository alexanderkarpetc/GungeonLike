using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace GamePlay.UI
{
    public class CameraFade : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField ] private float _fadeTime = 1f;
        [FormerlySerializedAs("_fadeValueEnd")] [SerializeField ] private float _visionValueEnd = 20f;
    
        private Material _material;

        private void Start()
        {
            _material = new Material(_spriteRenderer.material);
            _spriteRenderer.material = _material;
        }

        public void MakeFade()
        {
            Fade().Forget();
        }

        private async UniTask Fade()
        {
            await UniTask.Yield(PlayerLoopTiming.Update);
            float visionValue = 0;
            while (visionValue < _visionValueEnd)
            {
                visionValue += Time.deltaTime * _visionValueEnd / _fadeTime;
                _material.SetFloat("_Vision", visionValue);
                await UniTask.Yield();
            }
        }
    }
}