using Unity.Netcode;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour
{
    private readonly NetworkVariable<NetworkPlayerData> _netState = new(writePerm: NetworkVariableWritePermission.Owner);

    private Vector3 _vel;
    
    [Header("References")]
    [SerializeField] private Transform playerHead;
    
    [SerializeField] private float cheapInterpolationTime = 0.1f;

    private void Update()
    {
        if (IsOwner)
        {
            var trans = transform;
            _netState.Value = new NetworkPlayerData()
            {
                Position = trans.position,
                BodyRotation = trans.rotation.eulerAngles,
                HeadRotation = playerHead.rotation.eulerAngles
            };
        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, _netState.Value.Position, ref _vel,cheapInterpolationTime);
            transform.rotation = Quaternion.Euler(_netState.Value.BodyRotation);
            playerHead.rotation = Quaternion.Euler(_netState.Value.HeadRotation);
        }
    }

    private struct NetworkPlayerData : INetworkSerializable
    {
        private float _x, _y, _z;
        private float _xRot, _yRot;

        internal Vector3 Position
        {
            get => new(_x, _y, _z);
            set
            {
                _x = value.x;
                _y = value.y;
                _z = value.z;
            }
        }

        internal Vector3 BodyRotation
        {
            get => new(0, _yRot, 0);
            set => _yRot = value.y;
        }
        
        internal Vector3 HeadRotation
        {
            get => new(_xRot, _yRot, 0);
            set
            {
                _xRot = value.x;
                _yRot = value.y;
            }
        }
        
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _x);
            serializer.SerializeValue(ref _y);
            serializer.SerializeValue(ref _z);
            
            serializer.SerializeValue(ref _xRot);
            serializer.SerializeValue(ref _yRot);
        }
    }
}
