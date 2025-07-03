using Cysharp.Threading.Tasks;
using GamePlay.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private TMP_Text _healthValue;
        private PlayerState _playerState;

        private void Start()
        {
            Init().Forget();
        }

        private async UniTask Init()
        {
            await UniTask.WaitUntil(() => AppModel.PlayerGameObj() != null);
            _playerState = AppModel.PlayerState();
            _playerState.CurrentHp.OnValueChanged += UpdateHealth;
            _playerState.MaxHp.OnValueChanged += UpdateMaxHealth;
            UpdateMaxHealth(0, _playerState.MaxHp.Value);
            UpdateHealth(0, _playerState.CurrentHp.Value);
        }

        private void UpdateHealth(int previousValue, int newValue)
        {
            _healthValue.text = newValue.ToString();
            _slider.value = newValue;
        }

        private void UpdateMaxHealth(int previousValue, int newValue)
        {
            _slider.maxValue = newValue;
            // todo: hack in case maxHp comes first for some reason
            _slider.value = _playerState.CurrentHp.Value;
        }
    }
}