using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.Connection;
using MLAPI.SceneManagement;
public class ServerManager : NetworkedBehaviour
{
    private static ServerManager _instance;
    public static ServerManager Instance { get { return _instance; } }

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

    public List<PlayerController> playerControllers = new List<PlayerController>();
    public List<string> playerNames = new List<string>();
    public GameObject GuardPrefab, PrisonerPrefab;

    void Start()
    {

    }


    void Update()
    {
        //This is only working for host
        if(NetworkingManager.Singleton.IsHost)
        {
            InvokeServerRpc(Server_UpdatePlayerList);
        }
    }

    public void changeName(ulong clientID, string nameChange)
    {
        InvokeServerRpc(Server_PlayerNameChange, clientID, nameChange);
    }

    public void StartGame(string sceneName)
    {
        InvokeServerRpc(Server_StartGame, sceneName);
    }

    [ClientRPC]
    public void Client_UpdatePlayerList(string[] players)
    {
        Debug.Log("inside Client_UpdatePlayerList");
        Debug.Log(players[0]);
        if(IsServer)
        {
            return;
        }

        List<string> temp = new List<string>();
        foreach(string s in players)
        {
            temp.Add(s);
        }

        playerNames = temp;
    }


    [ServerRPC(RequireOwnership = false)]
    public void Server_UpdatePlayerList()
    {
        /*
        if (NetworkingManager.Singleton.ConnectedClientsList.Count > 0)
        {
            playerControllers.RemoveAll(item => item == null);
            playerNames.RemoveAll(item => item == null);
            foreach (NetworkedClient client in NetworkingManager.Singleton.ConnectedClientsList)
            {
                if (client.PlayerObject.GetComponent<PlayerController>())
                {
                    if (!playerControllers.Contains(client.PlayerObject.GetComponent<PlayerController>()))
                    {
                        playerControllers.Add(client.PlayerObject.GetComponent<PlayerController>());
                        playerNames.Add(client.PlayerObject.GetComponent<PlayerController>().playerName);
                    }
                }
            }
        }

        string[] clientPlayerList = playerNames.ToArray();
        InvokeClientRpcOnEveryone(Client_UpdatePlayerList, clientPlayerList);
        */
    }

    [ServerRPC(RequireOwnership = false)]
    public void Server_PlayerNameChange(ulong owner, string nameChange)
    {
        /*
        foreach (NetworkedClient NC in NetworkingManager.Singleton.ConnectedClientsList)
        {
            if(NC.ClientId == owner)
            {
                int index = playerNames.FindIndex(name => name == NC.PlayerObject.GetComponent<PlayerController>().playerName);
                playerNames.RemoveAt(index);
                NC.PlayerObject.GetComponent<PlayerController>().playerName = nameChange;
                playerNames.Add(NC.PlayerObject.GetComponent<PlayerController>().playerName);
            }
        }
        */
    }


    [ServerRPC(RequireOwnership = false)]
    public void Server_StartGame(string sceneName)
    {
        foreach (PlayerController pc in GameObject.FindObjectsOfType<PlayerController>())
        {
            //Check NetworkedVar<int> to decide enum
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
        InvokeClientRpcOnEveryone(Client_StartGame);



        NetworkSceneManager.SwitchScene(sceneName);
       
    }

    public void OnSceneSwitched()
    {
        foreach (PlayerController pc in GameObject.FindObjectsOfType<PlayerController>())
        {
            pc.SpawnPlayerGameStart();
        }
    }

    [ClientRPC]
    public void Client_StartGame()
    {
        foreach(PlayerController pc in GameObject.FindObjectsOfType<PlayerController>())
        {
            //Check NetworkedVar<int> to decide enum
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
