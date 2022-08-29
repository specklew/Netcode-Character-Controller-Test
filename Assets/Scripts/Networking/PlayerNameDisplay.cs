using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Networking
{
    public class PlayerNameDisplay : NetworkBehaviour
    {
        public NetworkVariable<FixedString32Bytes> displayName = new();
        
        [SerializeField] private float raycastDistance = 3f;
        
        private Transform _cameraTransform;
        private TMP_Text _displayNameText;
        private int _raycastLayer;

        private void Awake()
        {
            _raycastLayer = 1 << LayerMask.NameToLayer("Default");
        }

        private void Start()
        {
            ConnectComponents();
            
            if (!IsServer) return;

            PlayerData? playerData = ServerGameNetPortal.Instance.GetPlayerData(OwnerClientId);

            if (playerData.HasValue)
            {
                displayName.Value = playerData.Value.PlayerName;
            }
        }

        private void Update()
        {
            if (IsLocalPlayer) 
            {
                DisplayEnemyName();
            }
        }

        private void OnEnable()
        {
            GameplayManager.OnPlayerKilled += HandlePlayerKilled;
            
            ConnectComponents();
        }

        private void OnDisable()
        {
            GameplayManager.OnPlayerKilled -= HandlePlayerKilled;
        }
        
        private void DisplayEnemyName()
        {
            if (Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out RaycastHit hit, raycastDistance, _raycastLayer, QueryTriggerInteraction.Ignore))
            {
                var enemyScript = hit.transform.GetComponent<PlayerNameDisplay>();
                if (enemyScript != null)
                {
                    _displayNameText.text = enemyScript.displayName.Value.ToString();
                    _displayNameText.alpha = 1;
                    return;
                }
            }
            _displayNameText.alpha = 0;
        }

        private void ConnectComponents()
        {
            if (!IsLocalPlayer) return;
            _displayNameText = GetComponentInChildren<TMP_Text>();
            _cameraTransform = GetComponentInChildren<Camera>().transform;
        }

        private void HandlePlayerKilled(ulong playerId)
        {
            ConnectComponents();
        }
    }
}