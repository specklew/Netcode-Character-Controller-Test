using Unity.Netcode;
using UnityEngine;

namespace PlayerScripts
{
    [RequireComponent(typeof(PlayerMovement))]
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

        [Header("Interaction Settings")] 
        public float interactionRaycastDistance = 1.5f;
        
        [HideInInspector] public new Camera camera;
        
        private CharacterController _controller;
        private AudioListener _listener;

        public override void OnNetworkSpawn()
        {
            camera = GetComponentInChildren<Camera>();
            _listener = GetComponentInChildren<AudioListener>();
            _controller = GetComponent<CharacterController>();

            //TODO: Refactor to make it more readable
            
            if (IsOwner) return;

            camera.enabled = false;
            Destroy(_listener);

            if (IsServer) return;
            
            Destroy(camera.gameObject);
            Destroy(_controller);
        }
    }
}