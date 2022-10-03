using Unity.Netcode;
using UnityEngine;

namespace PlayerScripts
{
    [RequireComponent(typeof(Player))]
    public class PlayerMovement : NetworkBehaviour
    {
    
        [Header("References")]
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private Transform playerHead;

        private float _speed;
        private float _animationBlend;
        private float _targetRotation;
        private float _rotationVelocity;
        private float _verticalVelocity;
    
        private float _fallTimeoutDelta;

        private Animator _animator;
        private CharacterController _controller;
        private Camera _camera;
        private AudioListener _listener;
        private Player _player;
        private PlayerManagerInput _input;

        private bool _hasAnimator;
        private Vector2 _inputLook;
        private bool _inputSprint;
        private Vector2 _inputMove;
        private bool _inputJump;
        private float _cameraAngle;
    
        private float _pitch;
        
        private static readonly int VelocityX = Animator.StringToHash("velocity_x");
        private static readonly int VelocityY = Animator.StringToHash("velocity_y");

        private NetworkVariable<float> headRotation = new();

        private void Awake()
        {
            _player = GetComponent<Player>();
            _controller = GetComponent<CharacterController>();
            _animator = GetComponentInChildren<Animator>();
        }

        private void OnEnable()
        {
            _input = FindObjectOfType<PlayerManagerInput>();
        }

        private void Update()
        {
            if (IsServer)
            {
                UpdateServer();    
            }
            
            if(!IsOwner) return;
            
            HandleInput();
        }

        private void FixedUpdate()
        {
            if(!IsOwner) return;
            
            Jump();
            MoveClient();
            LookClient();
        }

        private void UpdateServer()
        {
            playerHead.localRotation = Quaternion.Euler(headRotation.Value, 0, 0);
        }

        private void HandleInput()
        {
            _inputLook = _input.look;
            _inputSprint = _input.sprint;
            _inputJump = _input.jump;
            _inputMove = _input.move;
            //TODO: Add analog movement handling
        }

        private void MoveClient()
        {
            _speed = _inputSprint ? _player.sprintSpeed : _player.moveSpeed;

            Vector3 inputDirection = new Vector3(_inputMove.x, 0.0f, _inputMove.y).normalized;
            inputDirection = transform.TransformDirection(inputDirection);

            Vector3 movementDirection = inputDirection * (_speed * Time.deltaTime) + 
                                        new Vector3(0, _verticalVelocity, 0) * Time.deltaTime;

            _controller.Move(movementDirection);

            //Send animation data to server.
            AnimatePlayerServerRPC(new Vector2(
                _speed * inputDirection.magnitude,
                _verticalVelocity * (_controller.isGrounded ? 0 : 1)));
        }

        private void Jump()
        {
            if (_controller.isGrounded){
        
                if (_inputJump)
                {
                    _verticalVelocity = Mathf.Sqrt(_player.jumpHeight * -2f * _player.gravity);
                }
            }
            else
            {
                if (_verticalVelocity < _player.terminalVelocity)
                {
                    _verticalVelocity += _player.gravity * Time.deltaTime;
                }    
            }
        }
    
        private void LookClient()
        {
            float mouseX = _inputLook.x * _player.mouseSensitivity * 0.022f;
            float mouseY = _inputLook.y * _player.mouseSensitivity * 0.022f;

            _pitch -= mouseY;
            _pitch = Mathf.Clamp(_pitch, _player.minPitch, _player.maxPitch);

            transform.Rotate(0, mouseX, 0);

            UpdateHeadRotationServerRPC(_pitch);
            cameraTransform.localRotation = Quaternion.Euler(_pitch, 0, 0);
        }
        
        
        [ServerRpc]
        private void AnimatePlayerServerRPC(Vector2 velocity)
        {
            _animator.SetFloat(VelocityX, velocity.x);
            _animator.SetFloat(VelocityY, velocity.y);
        }

        [ServerRpc]
        private void UpdateHeadRotationServerRPC(float rotation)
        {
            headRotation.Value = rotation;
        }
    }
}
