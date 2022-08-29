using System.Collections;
using SceneManagement;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RoundState.States
{
    public class RoundEnd : State
    {
        private const string SceneName = "LobbyScene";
        
        public RoundEnd(GameplayManager gameplayManager) : base(gameplayManager)
        {
        }

        public override IEnumerator Start()
        {
            GameplayManager.MonsterObject.Despawn(true);
            SendWinnerMessage();
            yield return new WaitForSeconds(10.0f);

            NetworkManager.Singleton.SceneManager.LoadScene(SceneName, LoadSceneMode.Single);
            GameplayManager.SetLobbyState();
        }

        private void SendWinnerMessage()
        {
            ulong playerId = GameplayManager.AlivePlayers[0];
            NetworkManager.Singleton.ConnectedClients[playerId].PlayerObject
                .GetComponent<PlayerNetworkHandler>().RoundEndClientRpc(playerId);
        }
    }
}