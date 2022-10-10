using Unity.Netcode;
using UnityEngine;

public class InteractableObject : NetworkBehaviour
{
    private enum Strategy{Pickup, Use}

    [SerializeField] private Strategy strat = Strategy.Pickup;
    
    [HideInInspector] public InteractionStrategy strategy;

    private void Awake()
    {
        SetStrategy();
    }

    private void SetStrategy()
    {
        strategy = strat switch
        {
            Strategy.Pickup => gameObject.AddComponent<PickupStrategy>(),
            Strategy.Use => gameObject.AddComponent<UseStrategy>(),
            _ => strategy
        };
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
        strategy.PerformInteraction();
    }
}
