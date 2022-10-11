using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;

namespace Interaction
{
    [ExecuteAlways]
    [RequireComponent(typeof(NetworkObject))]
    public class InteractableObject : NetworkBehaviour
    {
        public enum Strategies
        {
            Pickup,
            Use
        }

        public Strategies strategy;
        
        //[SerializeReference] public List<InteractionStrategy> strategies = new();
        
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
            // foreach (InteractionStrategy strategy in strategies)
            // {
            //     strategy.PerformInteraction();
            // }
        }
    }
}
