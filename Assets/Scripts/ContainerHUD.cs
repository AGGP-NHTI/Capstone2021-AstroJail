using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI.Messaging;
using MLAPI;
using System.ComponentModel;

public class ContainerHUD : NetworkedBehaviour
{
    Container _container;
    PlayerController _player;
    GameObject ContainerHUDPanel;
    List<ItemDefinition> PlayerInv;
    List<ItemDefinition> ContainerInv;

    //make sure the button corresponds with the number (we might need a list of buttons)

    // Start is called before the first frame update
    void Start()
    {
        // access both containers to get the items for both the player and container (should have sprites associated with them)
        //      Set PlayerInv and ContainerInv to there corresponding accessed containers
        //      Populate UI with those items 
        // Make Container Panel Appear
    }


    public void TakeItem()
    {
        //use ItemAt
        //get the integer of where the item is at the list
        //pass the int to the TakeItem() function
        //Will return the item to add
        //PlayerInventory.Add()
    }

    public void AddItem()
    {
        //Hud needs to remove item from the player inventory
        //Needs to add the item to the container inventory

    }

}
