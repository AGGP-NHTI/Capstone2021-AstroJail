using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MLAPI;
using MLAPI.Transports.UNET;
using TMPro;

public class MainMenuManager : NetworkBehaviour
{
    public GameObject instructionsPanel, mainMenuPanel, CreditsPanel, joinPanel, lobbyPanel, startButton;
    public TMP_InputField NameInputField, IPInputField;
    public TextMeshProUGUI prisonerList, guardList;
    private string tempGuardList, tempPrisonerList;
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

        if (lobbyPanel.activeSelf)
        {
            tempGuardList = "";
            tempPrisonerList = "";
            foreach (PlayerController pc in GameObject.FindObjectsOfType<PlayerController>())
            {
                if(pc.playerEnum.Value == 0)
                {
                    tempPrisonerList += pc.playerName.Value;
                    tempPrisonerList += ", ";
                }
                else if(pc.playerEnum.Value == 1)
                {
                    tempGuardList += pc.playerName.Value;
                    tempGuardList += ", ";
                }

            }
            guardList.text = tempGuardList;
            prisonerList.text = tempPrisonerList; 
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
        NetworkManager.Singleton.NetworkConfig.EnableSceneManagement = true;
        NetworkManager.Singleton.StartHost();
    }

    public void JoinJaredButton()
    {
        joinPanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        CreditsPanel.SetActive(false);
        instructionsPanel.SetActive(false);
        lobbyPanel.SetActive(true);
        NetworkManager.Singleton.gameObject.GetComponent<UNetTransport>().ConnectAddress = "66.31.95.85";
        NetworkManager.Singleton.StartClient();
    }

    public void JoinZachButton()
    {
        joinPanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        CreditsPanel.SetActive(false);
        instructionsPanel.SetActive(false);
        lobbyPanel.SetActive(true);
        NetworkManager.Singleton.gameObject.GetComponent<UNetTransport>().ConnectAddress = "73.149.137.68";
        NetworkManager.Singleton.StartClient();
    }

    public void changeName()
    {
        if (NameInputField.text.Length > 0)
        {
            string nameChange = NameInputField.text;
            foreach(PlayerController go in GameObject.FindObjectsOfType<PlayerController>())
            {
                if (go.myController)
                {
                    go.playerName.Value = NameInputField.text;
                }
            }
        }
    }

    public void swapGuard()
    {
        foreach (PlayerController go in GameObject.FindObjectsOfType<PlayerController>())
        {
            if (go.myController)
            {
                go.playerEnum.Value = 1;
            }
        }
    }

    public void swapPrisoner()
    {
        foreach (PlayerController go in GameObject.FindObjectsOfType<PlayerController>())
        {
            if (go.myController)
            {
                go.playerEnum.Value = 0;
            }
        }
    }

    public void StartGame()
    {
        ServerManager.Instance.StartGame("SampleScenezach");
    }
    public void Close()
    {
        Application.Quit();
    }
}
