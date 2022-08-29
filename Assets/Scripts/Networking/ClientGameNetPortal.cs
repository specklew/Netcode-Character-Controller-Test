using System;
using System.Text;
using Unity.Netcode;
using Unity.Netcode.Transports.UNET;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Networking
{
    [RequireComponent(typeof(GameNetPortal))]
    public class ClientGameNetPortal : MonoBehaviour
    {
        public static ClientGameNetPortal Instance => _instance;
        private static ClientGameNetPortal _instance;

        public DisconnectReason DisconnectReason { get; private set; } = new DisconnectReason();

        public event Action<ConnectStatus> OnConnectionFinished;

        public event Action OnNetworkTimedOut;

        private GameNetPortal _gameNetPortal;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            _gameNetPortal = GetComponent<GameNetPortal>();

            _gameNetPortal.OnNetworkReadied += HandleNetworkReadied;
            _gameNetPortal.OnConnectionFinished += HandleConnectionFinished;
            _gameNetPortal.OnDisconnectReasonReceived += HandleDisconnectReasonReceived;
            NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnect;
        }

        private void OnDestroy()
        {
            if (_gameNetPortal == null) { return; }

            _gameNetPortal.OnNetworkReadied -= HandleNetworkReadied;
            _gameNetPortal.OnConnectionFinished -= HandleConnectionFinished;
            _gameNetPortal.OnDisconnectReasonReceived -= HandleDisconnectReasonReceived;

            if (NetworkManager.Singleton == null) { return; }

            NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnect;
        }

        public void StartClient(string ipAdress)
        {
            string payload = JsonUtility.ToJson(new ConnectionPayload()
            {
                clientGuid = Guid.NewGuid().ToString(),
                clientScene = SceneManager.GetActiveScene().buildIndex,
                playerName = PlayerPrefs.GetString("PlayerName", "Missing Name")
            });

            byte[] payloadBytes = Encoding.UTF8.GetBytes(payload);

            UNetTransport transport = NetworkManager.Singleton.GetComponent<UNetTransport>();
            
            transport.ConnectAddress = ipAdress.Length < 1 ? "127.0.0.1" : ipAdress;

            NetworkManager.Singleton.NetworkConfig.ConnectionData = payloadBytes;

            NetworkManager.Singleton.StartClient();
        }

        private void HandleNetworkReadied()
        {
            if (!NetworkManager.Singleton.IsClient) { return; }

            if (!NetworkManager.Singleton.IsHost)
            {
                _gameNetPortal.OnUserDisconnectRequested += HandleUserDisconnectRequested;
            }
        }

        private void HandleUserDisconnectRequested()
        {
            DisconnectReason.SetDisconnectReason(ConnectStatus.UserRequestedDisconnect);
            
            NetworkManager.Singleton.Shutdown();

            HandleClientDisconnect(NetworkManager.Singleton.LocalClientId);

            SceneManager.LoadScene("Menu");
        }

        private void HandleConnectionFinished(ConnectStatus status)
        {
            if (status != ConnectStatus.Success)
            {
                DisconnectReason.SetDisconnectReason(status);
            }

            OnConnectionFinished?.Invoke(status);
        }

        private void HandleDisconnectReasonReceived(ConnectStatus status)
        {
            DisconnectReason.SetDisconnectReason(status);
        }

        private void HandleClientDisconnect(ulong clientId)
        {
            if (NetworkManager.Singleton.IsConnectedClient || NetworkManager.Singleton.IsHost) return;
            
            _gameNetPortal.OnUserDisconnectRequested -= HandleUserDisconnectRequested;

            if (SceneManager.GetActiveScene().name != "Menu")
            {
                if (!DisconnectReason.HasTransitionReason)
                {
                    DisconnectReason.SetDisconnectReason(ConnectStatus.GenericDisconnect);
                }

                SceneManager.LoadScene("Menu");
            }
            else
            {
                OnNetworkTimedOut?.Invoke();
            }
        }
    }
}