using PlayerScripts;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerInteraction : NetworkBehaviour
{
    private Player _player;
    
    private RaycastHit _hit;
    private Collider _lastCollider;
    
    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    void Update()
    {
        if (!IsOwner) return;

        if (Physics.Raycast(_player.camera.transform.position, _player.camera.transform.forward, out var hit, _player.interactionRaycastDistance))
        {
            if (hit.collider == _lastCollider) return; //If raycast hits the same object return.
            
            //Disable highlight on the last collider.
            if (_lastCollider != null)
            {
                InteractableObject interact1 = _lastCollider.GetComponentInParent(typeof(InteractableObject)) as InteractableObject;
                if (interact1 != null)
                {
                    interact1.DisableHighlight();
                }
            }
            
            InteractableObject interact2 = hit.collider.GetComponentInParent(typeof(InteractableObject)) as InteractableObject;
            if (interact2 != null)
            {
                interact2.EnableHighlight();
            }
            
            _lastCollider = hit.collider;
        }
        else
        {
            if (_lastCollider == null) return;
            
            //If the raycast leaves the collider disable highlight.
            InteractableObject interact = _lastCollider.GetComponentInParent(typeof(InteractableObject)) as InteractableObject;
            if (interact != null)
            {
                interact.DisableHighlight();
            }

            _lastCollider = null;
        }
    }
}
