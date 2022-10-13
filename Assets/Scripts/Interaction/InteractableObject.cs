using PlayerScripts;
using Unity.Netcode;
using UnityEngine;

namespace Interaction
{
    [RequireComponent(typeof(NetworkObject))]
    public class InteractableObject : NetworkBehaviour
    {
        private InteractionStrategy _interactionStrategy;

        private void Awake()
        {
            _interactionStrategy = GetComponent(typeof(InteractionStrategy)) as InteractionStrategy;
            if(_interactionStrategy == null) Debug.LogError("The interaction strategy has not been added to " + gameObject + "!");
        }

        public void EnableHighlight()
        {
            ScriptingHelper.SetLayerRecursively(gameObject, LayerMask.NameToLayer("Outline"));
        }

        public void DisableHighlight()
        {
            ScriptingHelper.SetLayerRecursively(gameObject, LayerMask.NameToLayer("Default"));
        }
    
        public void Interact()
        {
            Debug.Log("Interaction!");
            if(_interactionStrategy.IsSingleUse) NetworkManager.LocalClient.PlayerObject.gameObject.GetComponent<PlayerInteraction>().HideInteractionEffects(this);
            InteractServerRPC(OwnerClientId);
        }

        public InteractionStrategy GetInteractionStrategy()
        {
            return _interactionStrategy;
        }

    
        [ServerRpc(RequireOwnership = false)]
        private void InteractServerRPC(ulong playerId)
        {
            Debug.Log("Interaction on server!");
            InteractClientRPC(playerId);
        }
    
        [ClientRpc]
        private void InteractClientRPC(ulong playerId)
        {
            Debug.Log("Interaction on client!");
            _interactionStrategy.PerformInteraction(playerId);
        }
    }
}
