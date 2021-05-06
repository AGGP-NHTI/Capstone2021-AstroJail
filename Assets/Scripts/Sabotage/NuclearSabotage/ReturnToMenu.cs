using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.SceneManagement;
using UnityEngine.SceneManagement;

public class ReturnToMenu : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        NetworkSceneManager.OnSceneSwitched += ServerManager.Instance.onHostDisconnect;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Menu()
    {
        if(IsServer)
        {
            NetworkSceneManager.SwitchScene("MainMenu");
        }
        else
        {
            NetworkManager.Singleton.StopClient();
            SceneManager.LoadScene(0);
        }

    }
}
