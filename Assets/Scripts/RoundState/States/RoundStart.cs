using System.Collections;
using System.Collections.Generic;
using Helpers;
using SceneManagement;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RoundState.States
{
    public class RoundStart : State
    {
        private const string SceneName = "Round";

        public RoundStart(GameplayManager gameplayManager) : base(gameplayManager)
        {
            
        }

        public override IEnumerator Start()
        {
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += ClientsLoadedLevel;
            
            LoadRoundLevel();
            SetAlivePlayer();
            yield break;
        }

        private void End()
        {
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted -= ClientsLoadedLevel;
            
            GameplayManager.SetRoundState();
        }

        private static void SetAlivePlayer()
        {
            foreach (NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
            {
                GameplayManager.Instance.AlivePlayers.Add(client.ClientId);
            }
        }

        private void LoadRoundLevel()
        {
            if (!NetworkManager.Singleton.IsServer) return;
            TogglePlayersMovement();
            NetworkManager.Singleton.SceneManager.LoadScene(SceneName, LoadSceneMode.Single);

            /*TODO Delete this comment when all works.
            while (!sceneSwitch.IsCompleted)
            {
                await Task.Delay(3);
            }

            if (!GameObject.Find("LoadScene").TryGetComponent(out LoadScene loadScene)) return;
            
            while (GameplayManager.LoadedPlayers.Count != NetworkManager.Singleton.ConnectedClients.Count)
            {
                await Task.Delay(3);
                loadScene.UpdatePlayerCountClientRpc((byte)GameplayManager.LoadedPlayers.Count, (byte)NetworkManager.Singleton.ConnectedClients.Count);
            }

            RandomizeSpawns();
            TogglePlayersMovement();
            SpawnMonster();
            
            GameplayManager.SetRoundState();*/
        } 
        
        private void ClientsLoadedLevel(string scenename, LoadSceneMode loadscenemode, List<ulong> clientscompleted, List<ulong> clientstimedout)
        {
            if(scenename != SceneName) return;

            RandomizeSpawns();
            TogglePlayersMovement();
            SpawnMonster();

            End();
        }
        
        private void RandomizeSpawns()
        {
            GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag(GameplayManager.spawnPointTag);
            int spawnPointCount = spawnPoints.Length;

            if (spawnPointCount <= 0) Debug.LogWarning("Couldn't find any spawn points with tag: " + GameplayManager.spawnPointTag);

            //Setting player spawn points out of the spawn point list.
            for (int i = 0; i < NetworkManager.Singleton.ConnectedClients.Count; i++)
            {
                NetworkObject player = NetworkManager.Singleton.ConnectedClientsList[i].PlayerObject;
                for (int j = 0; j < spawnPointCount; j++)
                {
                    if (!spawnPoints[j].TryGetComponent(out SpawnPoint spawn)) continue;
                    if (spawn.isTaken) continue;
                    if (Random.Range(0, 1) > j / spawnPointCount) continue;

                    spawnPointCount--;
                    spawn.isTaken = true;

                    Vector3 height = new Vector3(0, GameplayManager.spawnHeight, 0);
                    Vector3 spawnPos = spawnPoints[j].transform.position;
                    spawnPos += height;

                    player.GetComponent<PlayerNetworkHandler>().ChangePlayerPositionClientRpc(spawnPos);
                    
                    break;
                }
            }
            
            SetRandomPlayerAsIt();
        }

        private static void SetRandomPlayerAsIt()
        {
            NetworkClient client = RandomHelper<NetworkClient>.GetRandomOutOfReadOnlyList(NetworkManager.Singleton.ConnectedClientsList);
            GameplayManager.Instance.SetNewIt(client.ClientId);
        }
        
        private void SpawnMonster()
        {
            GameObject spawnPoint = RandomHelper<GameObject>.GetRandomOutOfList(GameObject.FindGameObjectsWithTag(GameplayManager.monsterSpawnTag));
            
            GameplayManager.SpawnMonster(spawnPoint.transform.position + Vector3.up);
        }
        
        private static void TogglePlayersMovement()
        {
            for (int i = 0; i < NetworkManager.Singleton.ConnectedClients.Count; i++)
            {
                NetworkObject playerObject = NetworkManager.Singleton.ConnectedClientsList[i].PlayerObject;
                playerObject.GetComponent<PlayerNetworkHandler>().ToggleFreezePlayerClientRpc();
            }
        }
    }
}