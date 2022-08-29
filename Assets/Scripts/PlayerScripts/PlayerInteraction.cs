using Unity.Netcode;
using UnityEngine;

namespace PlayerScripts
{
    public class PlayerInteraction : NetworkBehaviour, IClickable
    {
        [SerializeField] private Transform cameraTransform;
            
        [SerializeField] private float raycastDistance = 1f;

        private void Update()
        {
            if (!IsLocalPlayer) return;
            //if (Input.GetButtonDown("Fire1")) FireRaycast();
        }

        private void FireRaycast()
        {
            if(Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, raycastDistance))
            {
                if (hit.transform.TryGetComponent(out IClickable clickable))
                    clickable.ClickedByPlayerServerRpc(OwnerClientId);
            }
        }
        
        
        [ServerRpc(RequireOwnership = false)]
        public void ClickedByPlayerServerRpc(ulong interactingPlayerId)
        {
            if(OwnerClientId == interactingPlayerId) return;
            
            GameplayManager gameplayManager = GameplayManager.Instance;
            if (gameplayManager.CurrentlyTaggedPlayer == interactingPlayerId)
            {
                gameplayManager.SetNewIt(OwnerClientId);
            }
        }
    }
}