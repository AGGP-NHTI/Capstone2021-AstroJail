using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class ServerIntitialization : NetworkBehaviour
{
    public GameObject serverManagerPrefab;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void createServerManager()
    {
        CreateServerManagerServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void CreateServerManagerServerRpc()
    {
        GameObject serverManager = Instantiate(serverManagerPrefab);
        serverManager.GetComponent<NetworkObject>().Spawn();
    }
}
