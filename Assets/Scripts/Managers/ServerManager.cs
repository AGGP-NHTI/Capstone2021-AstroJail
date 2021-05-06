using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.Connection;
using MLAPI.SceneManagement;
public class ServerManager : NetworkBehaviour
{
    private static ServerManager _instance;
    public static ServerManager Instance { get { return _instance; } }
    private float hostDCtimer = 2f;
    private bool startTimer = false;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        NetworkSceneManager.OnSceneSwitched += OnSceneSwitched;
    }

    public GameObject GuardPrefab, PrisonerPrefab;

    void Start()
    {

    }

    private void Update()
    {
        if(startTimer)
        {
            hostDCtimer -= Time.deltaTime;
            if(hostDCtimer <= 0)
            {
                NetworkManager.Singleton.StopHost();
            }
        }
    }

    public void StartGame(string sceneName)
    {
        StartGameServerRpc(sceneName);
    }


    [ServerRpc(RequireOwnership = false)]
    public void StartGameServerRpc(string sceneName)
    {
        foreach (PlayerController pc in GameObject.FindObjectsOfType<PlayerController>())
        {
            //Check NetworkVariable<int> to decide enum
            if (pc.playerEnum.Value == 0)
            {
                pc.selectedPlayerType = PlayerType.Prisoner;
            }
            else
            {
                pc.selectedPlayerType = PlayerType.Guard;
            }

            //Check enum to decide prefab
            if (pc.selectedPlayerType == PlayerType.Prisoner)
            {
                pc.PSpawn = PrisonerPrefab;
            }
            else
            {
                pc.PSpawn = GuardPrefab;
            }

        }
        StartGameClientRpc();

        NetworkSceneManager.SwitchScene(sceneName);
    }

    public void onHostDisconnect()
    {
        if(IsServer)
        {
            startTimer = true;
        }
        
    }
    public void OnSceneSwitched()
    {
        if (!IsServer) return;
        foreach (PlayerController pc in GameObject.FindObjectsOfType<PlayerController>())
        {
            pc.SpawnPlayerGameStart();
        }
    }

    [ClientRpc]
    public void StartGameClientRpc()
    {
        foreach(PlayerController pc in GameObject.FindObjectsOfType<PlayerController>())
        {
            //Check NetworkVariable<int> to decide enum
            if (pc.playerEnum.Value == 0)
            {
                pc.selectedPlayerType = PlayerType.Prisoner;
            }
            else
            {
                pc.selectedPlayerType = PlayerType.Guard;
            }


            //Check enum to decide prefab
            if (pc.selectedPlayerType == PlayerType.Prisoner)
            {
                pc.PSpawn = PrisonerPrefab;
            }
            else
            {
                pc.PSpawn = GuardPrefab;
            }
        }
    }
}
