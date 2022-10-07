using PlayerScripts;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerInteraction : NetworkBehaviour
{
    private Player _player;
    
    private RaycastHit _hit;
    
    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    void Update()
    {
        if (!IsOwner) return;

        //Raycast testing code.
        if (Physics.Raycast(_player.camera.transform.position, _player.camera.transform.forward, out var newHit, _player.interactionRaycastDistance))
        {
            if (newHit.collider != _hit.collider)
            {
                newHit.collider.GetComponent<Renderer>().material.color = Color.blue;
                if(_hit.collider != null) _hit.collider.GetComponent<Renderer>().material.color = Color.white;
                _hit = newHit;
            }
            Debug.Log("Hit! " + _hit.collider);
        }
        else
        {
            if(_hit.collider != null) _hit.collider.GetComponent<Renderer>().material.color = Color.white;
            _hit = new RaycastHit();
        }
    }
}
