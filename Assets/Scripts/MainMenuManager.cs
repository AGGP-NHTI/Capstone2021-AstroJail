using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MLAPI;
using TMPro;

public class MainMenuManager : NetworkedBehaviour
{
    public GameObject instructionsPanel, mainMenuPanel, CreditsPanel, joinPanel, lobbyPanel, startButton;
    public TMP_InputField NameInputField;
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
        if (IsServer)
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
        NetworkingManager.Singleton.StartHost();
    }

    public void JoinButton()
    {
        joinPanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        CreditsPanel.SetActive(false);
        instructionsPanel.SetActive(false);
        lobbyPanel.SetActive(true);
    }

    public void changeName()
    {
        //NetworkingManager.Singleton.
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void Close()
    {
        Application.Quit();
    }
}
