using Steamworks;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Networking
{
    public class SteamNetworkUiView : MonoBehaviour
    {
        [SerializeField] private Button _startHostButton;
        [SerializeField] private Button _startClientButton;
        [SerializeField] private Button _stopButton;
        [SerializeField] private Button _copyIdButton;
        [SerializeField] private SteamManager _steamManager;
        [SerializeField] private SteamTransport _transport;
        [SerializeField] private GameObject _container;
        [SerializeField] private TMP_InputField _serverId;

        private void OnEnable()
        {
            _startHostButton.onClick.AddListener(StartHost);
            _startClientButton.onClick.AddListener(StartClient);
            _stopButton.onClick.AddListener(Stop);
            _copyIdButton.onClick.AddListener(CopyId);
        }

        private void OnDisable()
        {
            _startHostButton.onClick.RemoveListener(StartHost);
            _startClientButton.onClick.RemoveListener(StartClient);
            _stopButton.onClick.RemoveListener(Stop);
            _copyIdButton.onClick.RemoveListener(CopyId);
        }

        private void StartClient()
        {
            _transport.SetTargetSteamId(new CSteamID(ulong.Parse(_serverId.text)));
            NetworkManager.Singleton.StartClient();
            _container.SetActive(false);
        }

        private void StartHost()
        {
            NetworkManager.Singleton.StartHost();
            _container.SetActive(false);
        }

        private void Stop()
        {
            _steamManager.MarkQuitting();
            NetworkManager.Singleton.Shutdown(true);
        }

        private void CopyId()
        {
            GUIUtility.systemCopyBuffer = SteamUser.GetSteamID().ToString();
        }
    }
}