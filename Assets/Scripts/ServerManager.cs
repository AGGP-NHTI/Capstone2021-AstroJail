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

    }


    public List<PlayerController> playerControllers = new List<PlayerController>();
    public List<string> playerNames = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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
    }

    [ServerRPC(RequireOwnership = false)]
    public void Server_PlayerNameChange(ulong owner, string nameChange)
    {

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

    }


    [ServerRPC(RequireOwnership = false)]
    public void Server_StartGame(string sceneName)
    {
        NetworkSceneManager.SwitchScene(sceneName);

        foreach(PlayerController pc in playerControllers)
        {
            pc.myPawn.gameObject.transform.position = new Vector3(0,10,0);
        }    
    }

  

}
