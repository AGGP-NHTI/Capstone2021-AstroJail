using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Spawning;

public class TestServer : MonoBehaviour
{

    public void StartAsHost()
    {
        NetworkingManager.Singleton.StartHost();
        gameObject.SetActive(false);
    }

    public void JoinAsClient()
    {
        NetworkingManager.Singleton.StartClient();
        gameObject.SetActive(false);
    }




}
