using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneManagement
{
    public class LoadScene : NetworkBehaviour
    {
        [SerializeField] private GameObject loadingScreenCanvas;
        [SerializeField] private string sceneName;
        private byte _loadedPlayerCount;
        private byte _playersOnServer = 1;

        private void Start()
        {
            if (IsClient)
            {
                LoadPlayersSceneClient();
            }
        }

        [ClientRpc]
        public void UpdatePlayerCountClientRpc(byte playerCount, byte playerConnectedCount)
        {
            _loadedPlayerCount = playerCount;
            _playersOnServer = playerConnectedCount;
        }

        private async void LoadPlayersSceneClient()
        {
            var scene = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            scene.allowSceneActivation = false;
            while (scene.progress < 0.9f)
            {
                await Task.Delay(3);
            }
            
            scene.allowSceneActivation = true;

            while (_loadedPlayerCount != _playersOnServer)
            {
                await Task.Delay(3);
            }

            loadingScreenCanvas.SetActive(false);
        }
        
    }
}
