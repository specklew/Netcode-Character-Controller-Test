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
            InteractServerRPC();
        }

    
        [ServerRpc]
        private void InteractServerRPC()
        {
            Debug.Log("Interaction on server!");
            InteractClientRPC();
        }
    
        [ClientRpc]
        private void InteractClientRPC()
        {
            Debug.Log("Interaction on client!");
            _interactionStrategy.PerformInteraction();
        }
    }
}
