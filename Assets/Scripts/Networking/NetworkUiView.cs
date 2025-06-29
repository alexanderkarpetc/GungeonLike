using System;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;

namespace Networking
{
    public class NetworkUiView : MonoBehaviour
    {
        [SerializeField] private Button _startHostButton;
        [SerializeField] private Button _startClientButton;
        [SerializeField] private GameObject _container;
        [SerializeField] private TMP_InputField _ip;
        [SerializeField] private TMP_InputField _port;

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
            var ip = string.IsNullOrWhiteSpace(_ip.text) ? "127.0.0.1" : _ip.text;
            var port = string.IsNullOrWhiteSpace(_port.text) ? "7777" : _port.text;
            var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            transport.ConnectionData.Address = ip;
            transport.ConnectionData.Port = Convert.ToUInt16(port);
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