using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using UnityEngine.UI;

public class CraftingHUD : NetworkedBehaviour
{
 
    //this variable holds the GENERIC STASH CONTAINER

    public Button CraftedItems;
    public recipes craftecCheck;
    bool Crafting = true;

    //this variable holds the GENERIC STASH CONTAINER
    public Containers _container;
    public PlayerController _player;
    public CraftingStash stash;
    GameObject ContainerHUDPanel;
    List<ItemDefinition> PlayerInv;
    List<ItemDefinition> ContainerInv;

    public List<Button> Containerbuttons;
    public List<Button> Playerbuttons;


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
        PlayerPawn tempPawn = (PlayerPawn)_player.myPawn;
        _container.gameObject.GetComponent<CraftingStash>().Done();
        tempPawn.ObjectUsing = null;
    }

    public void CraftItem()
    {
        PlayerPawn tempPawn = (PlayerPawn)_player.myPawn;
        while (tempPawn.ObjectUsing)
        {

            foreach (ItemDefinition Pitems in _container.ItemsInContainer)
            {
                foreach (ItemDefinition items in craftecCheck.AllCraftedItems)
                {


                }

            }
        }

    }


    public void TakeItem(int i)
    {
        Debug.Log("Index where item should've been taken from " + i);
        PlayerPawn tempPawn = (PlayerPawn)_player.myPawn;
        PlayerInv = tempPawn.playerInventory.ItemsInContainer;
        if (!tempPawn)
        {
            // debug if no player is set dont do stuff
            return;
        }

        if (tempPawn.playerInventory.itemCount >= tempPawn.playerInventory.MaxItems)
        {
            Debug.LogError($"{tempPawn} inventory is full");
        }
        else
        {
            if (IsServer)
            {
                tempPawn.playerInventory.Additem(_container.ItemsInContainer[i]);
                stash.TakeItemRPC(_container.ItemsInContainer[i].itemId);
            }
            else
            {
                stash.TakeItemRPC(_container.ItemsInContainer[i].itemId);
                tempPawn.playerInventory.Additem(_container.TakeItem(i));
            }
        }

        UpdateList();
    }

    public void AddItem(int i)
    {
        PlayerPawn tempPawn = (PlayerPawn)_player.myPawn;
        PlayerInv = tempPawn.playerInventory.ItemsInContainer;
        if (!tempPawn)
        {
            // debug if no player is set dont do stuff
            return;
        }

        if (_container.ItemsInContainer.Count >= _container.MaxItems)
        {
            Debug.LogError($"{tempPawn} inventory is full");
        }
        else
        {
            if (IsServer)
            {
                stash.AddItemRPC(tempPawn.playerInventory.ItemsInContainer[i].itemId);
                tempPawn.playerInventory.TakeItem(i);
            }
            else
            {
                stash.AddItemRPC(tempPawn.playerInventory.ItemsInContainer[i].itemId);
                _container.Additem(tempPawn.playerInventory.TakeItem(i));
            }

        }
        UpdateList();
    }

    public void UpdateList()
    {
        int i = 0;
        PlayerPawn tempPawn = (PlayerPawn)_player.myPawn;
        PlayerInv = tempPawn.playerInventory.ItemsInContainer;

        //Containerbuttons.Clear();
        //Playerbuttons.Clear();

        for (int buttons = 0; buttons < 6; buttons++)
        {
            if (buttons < 2)
            {
                Containerbuttons[buttons].gameObject.SetActive(false);
                Containerbuttons[buttons].onClick.RemoveAllListeners();
            }

            Playerbuttons[buttons].gameObject.SetActive(false);
            Playerbuttons[buttons].onClick.RemoveAllListeners();

        }
        foreach (ItemDefinition items in _container.ItemsInContainer)
        {
            Containerbuttons[i].gameObject.SetActive(true);
            int x = new int();
            x = i;
            Containerbuttons[i].onClick.AddListener(() => TakeItem(x));
            Containerbuttons[i].image.sprite = items.imageArt;
            i++;

        }
        i = 0;
        foreach (ItemDefinition items in PlayerInv)
        {
            Playerbuttons[i].gameObject.SetActive(true);
            int x = new int();
            x = i;
            Playerbuttons[i].onClick.AddListener(() => AddItem(x));
            Playerbuttons[i].image.sprite = items.imageArt;
            i++;

        }


    }

}
