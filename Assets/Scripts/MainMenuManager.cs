using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject instructionsPanel, mainMenuPanel, CreditsPanel, joinPanel, hostPanel, clientPanel;
    // Start is called before the first frame update
    void Start()
    {
        mainMenuPanel.SetActive(true);
        CreditsPanel.SetActive(false);
        instructionsPanel.SetActive(false);
        joinPanel.SetActive(false);
        clientPanel.SetActive(false);
        hostPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Instructions()
    {
        mainMenuPanel.SetActive(false);
        CreditsPanel.SetActive(false);
        instructionsPanel.SetActive(true);
        joinPanel.SetActive(false);
        clientPanel.SetActive(false);
        hostPanel.SetActive(false);
    }
    public void Credits()
    {
        mainMenuPanel.SetActive(false);
        CreditsPanel.SetActive(true);
        instructionsPanel.SetActive(false);
        joinPanel.SetActive(false);
        clientPanel.SetActive(false);
        hostPanel.SetActive(false);
    }
    public void MainMenu()
    {
        mainMenuPanel.SetActive(true);
        CreditsPanel.SetActive(false);
        instructionsPanel.SetActive(false);
        joinPanel.SetActive(false);
        clientPanel.SetActive(false);
        hostPanel.SetActive(false);
    }

    public void PlayButton()
    {
        joinPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
        CreditsPanel.SetActive(false);
        instructionsPanel.SetActive(false);
        clientPanel.SetActive(false);
        hostPanel.SetActive(false);

    }
    public void HostButton()
    {
        joinPanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        CreditsPanel.SetActive(false);
        instructionsPanel.SetActive(false);
        clientPanel.SetActive(false);
        hostPanel.SetActive(true);

    }

    public void JoinButton()
    {
        joinPanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        CreditsPanel.SetActive(false);
        instructionsPanel.SetActive(false);
        clientPanel.SetActive(true);
        hostPanel.SetActive(false);

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
