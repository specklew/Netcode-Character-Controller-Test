using PlayerScripts;
using Unity.Netcode;
using Unity.VisualScripting;
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
                if (_lastCollider.TryGetComponent(out InteractableObject interact1))
                {
                    interact1.DisableHighlight();
                }
            }
            
            //Enable highlight on the new one.
            if (hit.collider.TryGetComponent(out InteractableObject interact2))
            {
                interact2.EnableHighlight();
            }
            
            _lastCollider = hit.collider;
        }
        else
        {
            if (_lastCollider == null) return;
            
            //If the raycast leaves the collider disable highlight.
            if (_lastCollider.TryGetComponent(out InteractableObject interactableObject))
            {
                interactableObject.DisableHighlight();
            }

            _lastCollider = null;
        }
    }
}
