using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MLAPI;
using MLAPI.Transports.UNET;
using TMPro;

public class MainMenuManager : NetworkedBehaviour
{
    public GameObject instructionsPanel, mainMenuPanel, CreditsPanel, joinPanel, lobbyPanel, startButton;
    public TMP_InputField NameInputField, IPInputField;
    // Start is called before the first frame update
    void Start()
    {
        mainMenuPanel.SetActive(true);
        CreditsPanel.SetActive(false);
        instructionsPanel.SetActive(false);
        joinPanel.SetActive(false);
        lobbyPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsHost)
        { 
            startButton.SetActive(true);
        }
        else
        {
            startButton.SetActive(false);
        }
    }
    public void Instructions()
    {
        mainMenuPanel.SetActive(false);
        CreditsPanel.SetActive(false);
        instructionsPanel.SetActive(true);
        joinPanel.SetActive(false);
        lobbyPanel.SetActive(false);
    }
    public void Credits()
    {
        mainMenuPanel.SetActive(false);
        CreditsPanel.SetActive(true);
        instructionsPanel.SetActive(false);
        joinPanel.SetActive(false);
        lobbyPanel.SetActive(false);
    }
    public void MainMenu()
    {
        mainMenuPanel.SetActive(true);
        CreditsPanel.SetActive(false);
        instructionsPanel.SetActive(false);
        joinPanel.SetActive(false);
        lobbyPanel.SetActive(false);
    }

    public void PlayButton()
    {
        joinPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
        CreditsPanel.SetActive(false);
        instructionsPanel.SetActive(false);
        lobbyPanel.SetActive(false);

    }
    public void HostButton()
    {
        joinPanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        CreditsPanel.SetActive(false);
        instructionsPanel.SetActive(false);
        lobbyPanel.SetActive(true);
        NetworkingManager.Singleton.NetworkConfig.EnableSceneManagement = true;
        NetworkingManager.Singleton.StartHost();
    }

    public void JoinButton()
    {
        joinPanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        CreditsPanel.SetActive(false);
        instructionsPanel.SetActive(false);
        lobbyPanel.SetActive(true);
        NetworkingManager.Singleton.gameObject.GetComponent<UnetTransport>().ConnectAddress = "66.31.95.85";
        NetworkingManager.Singleton.StartClient();
    }

    public void changeName()
    {
        if (NameInputField.text.Length > 0)
        {
            string nameChange = NameInputField.text;
            ServerManager.Instance.changeName(NetworkingManager.Singleton.LocalClientId, nameChange);
            foreach(PlayerController go in GameObject.FindObjectsOfType<PlayerController>())
            {
                if (go.myController)
                {
                    go.playerName.Value = NameInputField.text;
                }
            }
        }
    }

    public void StartGame()
    {
        ServerManager.Instance.StartGame("SampleScene");
    }
    public void Close()
    {
        Application.Quit();
    }
}
