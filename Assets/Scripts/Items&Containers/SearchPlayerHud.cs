using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI.Messaging;
using MLAPI;
using System.ComponentModel;
using UnityEngine.UI;


public class SearchPlayerHud : NetworkBehaviour
{
    //this variable holds the GENERIC STASH CONTAINER
    public Containers _container;
    public PlayerController _player;
    GameObject ContainerHUDPanel;
    List<ItemDefinition> PlayerInv;
    List<ItemDefinition> ContainerInv;

    public List<Button> Containerbuttons;
    public List<Button> Playerbuttons;
    public bool failedSearch = false;
        


    //make sure the button corresponds with the number (we might need a list of buttons)
    void Start()
    {
        UpdateList();
    
        // access both containers to get the items for both the player and container (should have sprites associated with them)
        //      Set PlayerInv and ContainerInv to there corresponding accessed containers
        //      Populate UI with those items 
        // Make Container Panel Appear
    }

    public void CloseButtom()
    {
        GuardPawn temp = (GuardPawn)_player.myPawn;
        temp.DoneSearching();
        temp.ObjectUsing = null;
    }
    public void searchForContraband(int i)
    {   
        PlayerInv = _container.ItemsInContainer;
      

        if (PlayerInv.Count <= 0)
        {
            Debug.LogError($"{PlayerInv} inventory is empty");
            return;
        }
        else
        {
            if(_container.ItemsInContainer[i].isContraband)
            {
                //run rpc to send items back 
            }
            else
            {
                Debug.Log("found non contraband item" +"item you found was: " + _container.ItemsInContainer[i]);
                GuardPawn temp = (GuardPawn)_player.myPawn;
                temp.FailedSearch();

            }
            
           
        }

    }

    public void TakeItem(int i)
    {
       
            /*if (IsServer)
            {
                tempPawn.playerInventory.Additem(_container.ItemsInContainer[i]);
                stash.TakeItemRPC(_container.ItemsInContainer[i].instanceId);
            }
            else
            {
                stash.TakeItemRPC(_container.ItemsInContainer[i].instanceId);
                tempPawn.playerInventory.Additem(_container.TakeItem(i));
            }*/
        

        //WE SHOULD BE UPDATING THE LIST IN THE GUARDPAWN RPC
    }

    public void UpdateList()
    {
        int i = 0;
      //  PrisonerPawn tempPawn = (PrisonerPawn)_player.myPawn;
       // PlayerInv = tempPawn.playerInventory.ItemsInContainer;

        //Containerbuttons.Clear();
        //Playerbuttons.Clear();

        for (int buttons = 0; buttons < 6; buttons++)
        {
            
            Playerbuttons[buttons].gameObject.SetActive(false);
            Playerbuttons[buttons].onClick.RemoveAllListeners();
        }
        foreach (ItemDefinition items in _container.ItemsInContainer)
        {
            Playerbuttons[i].gameObject.SetActive(true);
            int x = new int();
            x = i;
            Playerbuttons[i].onClick.AddListener(() => TakeItem(x));
            Playerbuttons[i].image.sprite = items.imageArt;
            i++;
        }
        i = 0;
        
    }
}

