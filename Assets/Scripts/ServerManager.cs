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


    public List<PlayerController> players = new List<PlayerController>();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("test");
        InvokeServerRpc(Server_UpdatePlayerList);
    }

    [ClientRPC]
    public void Client_UpdatePlayerList()
    {
        if(IsServer)
        {
            return;
        }

        if (NetworkingManager.Singleton.ConnectedClientsList.Count > 0)
        {
            players.RemoveAll(item => item == null);
            foreach (NetworkedClient client in NetworkingManager.Singleton.ConnectedClientsList)
            {
                if (client.PlayerObject.GetComponent<PlayerController>())
                {
                    if (!players.Contains(client.PlayerObject.GetComponent<PlayerController>()))
                    {
                        players.Add(client.PlayerObject.GetComponent<PlayerController>());
                    }
                }
            }
        }
    }

    [ServerRPC(RequireOwnership = false)]
    public void Server_UpdatePlayerList()
    {


        if (NetworkingManager.Singleton.ConnectedClientsList.Count > 0)
        {
            players.RemoveAll(item => item == null);
            foreach (NetworkedClient client in NetworkingManager.Singleton.ConnectedClientsList)
            {
                if (client.PlayerObject.GetComponent<PlayerController>())
                {
                    if (!players.Contains(client.PlayerObject.GetComponent<PlayerController>()))
                    {
                        players.Add(client.PlayerObject.GetComponent<PlayerController>());
                    }
                }
            }
        }
        InvokeClientRpcOnEveryone(Client_UpdatePlayerList);
    }

}
