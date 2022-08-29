using System;
using RoundState.States;
using SceneManagement;
using Unity.Netcode;
using UnityEngine;
using StateMachine = RoundState.StateMachine;

public class GameplayManager : StateMachine
{
    public static GameplayManager Instance;

    [Header("Player Spawning")]
    public string spawnPointTag = "SpawnPoint";
    public float spawnHeight = 10f;
    public GameObject ghostPrefab;

    [Header("Monster Spawning")]
    public string monsterSpawnTag = "MonsterSpawn";
    public NetworkObject monsterPrefab;

    public NetworkObject MonsterObject { get; private set; }

    public static event Action<ulong> OnPlayerKilled;
    public static event Action<ulong> OnPlayerSetAsIt;
    
    public ulong CurrentlyTaggedPlayer { get; private set; }
    
    public NetworkList<ulong> AlivePlayers;

    private void InvokeOnPlayerKilled(ulong clientId) => OnPlayerKilled?.Invoke(clientId);
    private void InvokeOnPlayerSetAsIt(ulong clientId) => OnPlayerSetAsIt?.Invoke(clientId);
    
    private void Awake()
    {
       
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            AlivePlayers = new NetworkList<ulong>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SetLobbyState();
    }

    private void OnDestroy()
    {
        AlivePlayers.Dispose();
    }

    #region States

    public void SetRoundStartState()
    {
        SetState(new RoundStart(this));
    }
    
    public void SetRoundState()
    {
        SetState(new Round(this));
    }
    
    public void SetRoundEndState()
    {
        SetState(new RoundEnd(this));
    }
    
    public void SetLobbyState()
    {
        SetState(new InLobby(this));
    }

    #endregion
    
    #region SceneManagement
    
    public void SpawnMonster(Vector3 spawnPosition)
    {
        MonsterObject = Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
        MonsterObject.Spawn();
    }

    #endregion

    #region GameLogic
    
    public void SetNewIt(ulong playerId)
    {
        CurrentlyTaggedPlayer = playerId;
        for (int i = 0; i < NetworkManager.Singleton.ConnectedClients.Count; i++)
        {
            NetworkObject playerObject = NetworkManager.Singleton.ConnectedClientsList[i].PlayerObject;
            InvokeOnPlayerSetAsIt(playerId);
            
            if (playerObject.TryGetComponent(out PlayerNetworkHandler playerNetworkHandler))
            {
                playerNetworkHandler.isTagged.Value = NetworkManager.Singleton.ConnectedClientsList[i].ClientId == playerId;
            }
        }
    }

    public void KillPlayer(ulong playerId)
    {
        AlivePlayers.Remove(playerId);
        CheckIfGameShouldEnd();
        
        Vector3 spawnVector = NetworkManager.Singleton.ConnectedClients[playerId].PlayerObject.transform.position;
        spawnVector += new Vector3(0, 2, 0);
        
        GameObject playerObject = Instantiate(ghostPrefab, spawnVector, Quaternion.identity);
        GameObject oldPlayerObject = NetworkManager.Singleton.ConnectedClients[playerId].PlayerObject.gameObject;
        
        playerObject.GetComponent<NetworkObject>().SpawnAsPlayerObject(playerId);
        
        InvokeOnPlayerKilled(playerId);
        
        Destroy(oldPlayerObject);
    }

    private void CheckIfGameShouldEnd()
    {
        if (AlivePlayers.Count <= 1)
        {
            SetRoundEndState();
        }
    }
    
    #endregion
}