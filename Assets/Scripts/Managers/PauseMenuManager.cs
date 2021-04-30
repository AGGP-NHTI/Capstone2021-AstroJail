using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MLAPI;
using MLAPI.Transports.UNET;
using TMPro;

public class PauseMenuManager : NetworkBehaviour
{
    public void ReturnToMain()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.StopHost();
            SceneManager.LoadScene(0);

        }
        else
        {
            NetworkManager.Singleton.Shutdown();
            NetworkManager.Singleton.StopClient();
            SceneManager.LoadScene(0);
        }
    }
    
}
