using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.Connection;

public class ServerManager : MonoBehaviour
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
        foreach(PlayerController pc in players)
        {
            if(pc == null)
            {
                players.Remove(pc);
            }
        }

        foreach(NetworkedClient client in NetworkingManager.Singleton.ConnectedClientsList)
        {
            if(client.PlayerObject.GetComponent<PlayerController>())
            {
                if(!players.Contains(client.PlayerObject.GetComponent<PlayerController>()))
                {
                    players.Add(client.PlayerObject.GetComponent<PlayerController>());
                }
            }
        }
    }
}
