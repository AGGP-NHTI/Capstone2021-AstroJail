using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI.Messaging;
using MLAPI;
using System.ComponentModel;
using UnityEngine.UI;

public class ContainerHUD : NetworkedBehaviour
{

    //this variable holds the GENERIC STASH CONTAINER
    public Containers _container;
    public PlayerController _player;
    GameObject ContainerHUDPanel;
    List<ItemDefinition> PlayerInv;
    List<ItemDefinition> ContainerInv;

    public List<Button> Containerbuttons;
    public List<Button> Playerbuttons;
    int i = 0;
  
    //make sure the button corresponds with the number (we might need a list of buttons)
    void Start()
    {
        UpdateList();

        // access both containers to get the items for both the player and container (should have sprites associated with them)
        //      Set PlayerInv and ContainerInv to there corresponding accessed containers
        //      Populate UI with those items 
        // Make Container Panel Appear
    }

   

    public void TakeItem()
    {
        UpdateList();
    }

    public void AddItem(int i)
    {
        UpdateList();

    }
    public void UpdateList()
    {
        PlayerPawn tempPawn = (PlayerPawn)_player.myPawn;
        PlayerInv = tempPawn.playerInventory.ItemsInContainer;


        foreach (ItemDefinition items in _container.ItemsInContainer)
        {
            Containerbuttons[i].gameObject.SetActive(true);
            Containerbuttons[i].onClick.AddListener(() => AddItem(i));
            Containerbuttons[i].image.sprite = items.imageArt;
            i++;
        }
        i = 0;
        foreach (ItemDefinition items in PlayerInv)
        {
            Playerbuttons[i].gameObject.SetActive(true);
            Playerbuttons[i].onClick.AddListener(() => AddItem(i));
            Playerbuttons[i].image.sprite = items.imageArt;
            i++;

        }
    }

}
