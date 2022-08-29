using Unity.Netcode;
using UnityEngine;

namespace PlayerScripts
{
    public class CharacterControllerPlayerMovement : NetworkBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private Camera cameraPlayer;
        [SerializeField] private Transform playerHead;
        [SerializeField] private CharacterController playerController;
        
        [Header("Mouse Settings")]
        [SerializeField] private float maxPitch = 85f;
        [SerializeField] private float minPitch = -85f;
        [SerializeField] private float mouseSensitivity = 3f;
        
        [Header("Movement Settings")]
        [SerializeField] private float playerSpeed = 10f;
        [SerializeField] private float sprintMultiplier = 3f;
        [SerializeField] private float jumpInitialSpeed = 2f;
        [SerializeField] private float gravity = 9.81f;
        [SerializeField] private float strafeMovementVectorMultiplier = 0.2f;

        [Header("Stamina")]
        [SerializeField] private float staminaStep = 0.1f;
        [SerializeField] private float staminaJump = 10f;
        public float Stamina { get; private set; } = 100.0f;
        
        private AudioListener _audioListener;
        private Animator _animator;
        private float _pitch;
        private Vector3 _movement;
        private static readonly int Speed = Animator.StringToHash("Speed");

        public override void OnNetworkSpawn()
        {
            playerController ??= GetComponent<CharacterController>();
            _audioListener = GetComponentInChildren<AudioListener>();
            _animator = GetComponentInChildren<Animator>();

            /*if (IsLocalPlayer) return;
            
            playerController.enabled = false;
            _audioListener.enabled = false;
            cameraPlayer.enabled = false;*/
        }

        public void Update()
        {
            if (!IsLocalPlayer) return;
            
            Look();
            Move();
        }

        private void Move()
        {
            Vector3 move = _movement;
            Vector3 movementInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            if (playerController.isGrounded)
            {
                move = new Vector3(movementInput.x, _movement.y, movementInput.z);
                move = transform.TransformDirection(move);
                move *= playerSpeed;

                if (Stamina >= staminaStep)
                {
                    if (Input.GetButton("Sprint"))
                    {
                        Stamina -= 5f * staminaStep; 
                        move *= sprintMultiplier;
                    }
            
                    if (Input.GetButton("Jump"))
                    {
                        Stamina -= staminaJump;
                        _movement.y = jumpInitialSpeed;
                    }
                }
            }
            else
            {
                Vector3 vector = strafeMovementVectorMultiplier * transform.TransformDirection(movementInput);
                float halfAngle = Vector3.Angle(vector, move)/2;
                move += vector * Mathf.Abs(Mathf.Sin(halfAngle));
            }

            playerController.Move(move * Time.deltaTime);
            _animator.SetFloat(Speed, playerController.velocity.magnitude);

            _movement.y -= gravity * Time.deltaTime;
            _movement = new Vector3(move.x, _movement.y, move.z);
            
            if(Stamina < 100.0f) Stamina += staminaStep;
        }

        private void Look()
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            transform.Rotate(0, mouseX, 0);

            _pitch -= mouseY;
            _pitch = Mathf.Clamp(_pitch, minPitch, maxPitch);

            Quaternion rot = Quaternion.Euler(_pitch, 0, 0);
            cameraTransform.localRotation = rot;
            playerHead.localRotation = rot;
        }
        
        public void ResetStamina()
        {
            Stamina = 100.0f;
        }
    }
}