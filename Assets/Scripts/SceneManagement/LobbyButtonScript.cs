using PlayerScripts;
using Unity.Netcode;

namespace SceneManagement
{
    public class LobbyButtonScript : NetworkBehaviour, IClickable
    {
        private bool _changeStarted;

        [ServerRpc(RequireOwnership = false)]
        public void ClickedByPlayerServerRpc(ulong playerId)
        {
            if (_changeStarted) return;
            
            _changeStarted = true;
            LoadServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        private void LoadServerRpc()
        {
            GameplayManager.Instance.SetRoundStartState();
        }
    }
}
