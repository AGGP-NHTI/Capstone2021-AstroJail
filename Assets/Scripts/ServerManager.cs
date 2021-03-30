using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.Connection;

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

        Debug.Log("test");
        InvokeServerRpc(Server_UpdatePlayerList);
    }

    public void changeName(ulong clientID, string nameChange)
    {
        InvokeServerRpc(Server_PlayerNameChange, clientID, nameChange);
    }

    [ClientRPC]
    public void Client_UpdatePlayerList(List<string> players)
    {
        if(IsServer)
        {
            return;
        }

        if (players.Count > 0)
        {
            playerNames.RemoveAll(item => item == null);
            foreach (string name in playerNames)
            {
                playerNames.Add(name);
            }
        }
    }

    [ServerRPC(RequireOwnership = false)]
    public void Server_UpdatePlayerList()
    {
        List<string> players = new List<string>();

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
                        players.Add(client.PlayerObject.GetComponent<PlayerController>().playerName);
                        playerNames.Add(client.PlayerObject.GetComponent<PlayerController>().playerName);
                    }
                }
            }
        }
        InvokeClientRpcOnEveryone(Client_UpdatePlayerList, players);
    }

    [ServerRPC(RequireOwnership = false)]
    public void Server_PlayerNameChange(ulong owner, string nameChange)
    {
        foreach(NetworkedClient NC in NetworkingManager.Singleton.ConnectedClientsList)
        {
            if(NC.ClientId == owner)
            {
                NC.PlayerObject.GetComponent<PlayerController>().playerName = nameChange;
            }
        }
    }

}
