using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject instructionsPanel, mainMenuPanel, CreditsPanel;
    // Start is called before the first frame update
    void Start()
    {
        mainMenuPanel.SetActive(true);
        CreditsPanel.SetActive(false);
        instructionsPanel.SetActive(false);
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
    }
    public void Credits()
    {
        mainMenuPanel.SetActive(false);
        CreditsPanel.SetActive(true);
        instructionsPanel.SetActive(false);
    }
    public void MainMenu()
    {
        mainMenuPanel.SetActive(true);
        CreditsPanel.SetActive(false);
        instructionsPanel.SetActive(false);
    }

    public void PlayButton()
    {
        SceneManager.LoadScene(1);
    }
    public void Close()
    {
        Application.Quit();
    }
}
