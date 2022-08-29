using PlayerScripts;
using Unity.Netcode;
using UnityEngine;

namespace SceneManagement
{
    public class PlayerNetworkHandler : NetworkBehaviour
    {
        public NetworkVariable<bool> isTagged = new();
        private bool _isFrozen;

        private void OnEnable()
        {
            isTagged.OnValueChanged += OnTaggedPlayerChange;
            UpdatePlayerTagIndication(isTagged.Value);
        }

        private void OnDisable()
        {
            isTagged.OnValueChanged -= OnTaggedPlayerChange;
            UpdatePlayerTagIndication(isTagged.Value);
        }
        
        [ClientRpc]
        public void ToggleFreezePlayerClientRpc()
        {
            if (!IsClient) return;
            if (!_isFrozen)
            {
                gameObject.GetComponent<CharacterController>().enabled = false;
                gameObject.GetComponent<CharacterControllerPlayerMovement>().enabled = false;
                _isFrozen = true;
            }
            else
            {
                gameObject.GetComponent<CharacterController>().enabled = true;
                gameObject.GetComponent<CharacterControllerPlayerMovement>().enabled = true;
                
                _isFrozen = false;
            }
        }

        [ClientRpc]
        public void ChangePlayerPositionClientRpc(Vector3 position)
        {
            transform.position = position;
        }
        
        [ClientRpc]
        public void RoundEndClientRpc(ulong winnerId)
        {
            Debug.Log("The winner is " + winnerId + "!");
        }

        private void OnTaggedPlayerChange(bool previous, bool current)
        {
            UpdatePlayerTagIndication(current);
        }

        private void UpdatePlayerTagIndication(bool tagged)
        {
            GetComponentInChildren<Renderer>().material.color = tagged ? Color.blue : Color.red;
        }
    }
}
