using Unity.Netcode;
using UnityEngine;

namespace PlayerScripts
{
    public class Player : NetworkBehaviour
    {
        [Header("Movement")] [Tooltip("Move speed of the character in m/s")]
        public float moveSpeed = 2.0f;

        [Tooltip("Sprint speed of the character in m/s")]
        public float sprintSpeed = 5.335f;

        [Space(10)] [Tooltip("The height the player can jump")] 
        public float jumpHeight = 1.2f;

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float gravity = -9.81f;
    
        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float terminalVelocity = 53.0f;

        [Header("Mouse Settings")]
        public float maxPitch = 85f;
        public float minPitch = -85f;
        public float mouseSensitivity = 3f;
        
        private Animator _animator;
        private Camera _camera;
        private CharacterController _controller;
        private AudioListener _listener;
        private PlayerMovement _movement;
        
        public override void OnNetworkSpawn()
        {
            _animator = GetComponentInChildren<Animator>();
            _camera = GetComponentInChildren<Camera>();
            _controller = GetComponent<CharacterController>();
            _listener = GetComponentInChildren<AudioListener>();
            _movement = GetComponent<PlayerMovement>();

            if (IsOwner) return;
        
            Destroy(_camera.gameObject);
            Destroy(_listener);
            Destroy(_controller);
        }

        private void Update()
        {
            UpdateServer();
            UpdateClient();
        }

        private void UpdateServer()
        {
            if(!IsServer) return;
        }
        
        private void UpdateClient()
        {
            if(!IsOwner) return;
        }
    }
}