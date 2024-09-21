using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Networking
{
    public class TemporaryUI : MonoBehaviour
    {
        [SerializeField] private Button _startHostButton;
        [SerializeField] private Button _startClientButton;
        [SerializeField] private GameObject _container;

        private void OnEnable()
        {
            _startHostButton.onClick.AddListener(StartHost);
            _startClientButton.onClick.AddListener(StartClient);
        }

        private void OnDisable()
        {
            _startHostButton.onClick.RemoveListener(StartHost);
            _startClientButton.onClick.RemoveListener(StartClient);
        }

        private void StartClient()
        {
            NetworkManager.Singleton.StartClient();
            _container.SetActive(false);
        }

        private void StartHost()
        {
            NetworkManager.Singleton.StartHost();
            _container.SetActive(false);
        }
    }
}