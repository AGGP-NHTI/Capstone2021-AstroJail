using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using UnityEngine.UI;

public class CraftingHUD : NetworkBehaviour
{

    //this variable holds the GENERIC STASH CONTAINER

    public Button CraftedItems;

    bool Crafting = true;

    //this variable holds the GENERIC STASH CONTAINER
    public Containers _container;
    public PlayerController _player;
    public CraftingStash stash;
    GameObject ContainerHUDPanel;
    List<ItemDefinition> PlayerInv;
    List<ItemDefinition> ContainerInv;

    public Dropdown dropDown;

    public List<Button> Containerbuttons;
    public List<Button> Playerbuttons;


    //make sure the button corresponds with the number (we might need a list of buttons)
    void Start()
    {
        UpdateList();
        dropDownList();
        stash.craftedItem = stash.CraftableItems[dropDown.value];
        dropDown.onValueChanged.AddListener(delegate
        {
            DropDownValueChanged(dropDown);
        });
        // access both containers to get the items for both the player and container (should have sprites associated with them)
        //      Set PlayerInv and ContainerInv to there corresponding accessed containers
        //      Populate UI with those items 
        // Make Container Panel Appear
    }

    public void CloseButtom()
    {
        PrisonerPawn tempPawn = (PrisonerPawn)_player.myPawn;
        _container.gameObject.GetComponent<CraftingStash>().Done();
        tempPawn.ObjectUsing = null;
    }

    public void CraftItem()
    {     
        int CorrectItems = 0;
        PrisonerPawn tempPawn = (PrisonerPawn)_player.myPawn;

        if (_container.ItemsInContainer.Count == stash.craftedItem.Recipe.Count)
        {
            foreach (ItemDefinition ingredients in stash.craftedItem.Recipe)
            {
                foreach(ItemDefinition items in _container.ItemsInContainer)
                {
                    Debug.Log($"igredients needed{ingredients} was compared to items{items}");
                    if(items.itemId == ingredients.itemId)
                    {
                        Debug.Log($"#correct item {CorrectItems} out of {ingredients}");
                        CorrectItems++;
                    }

                }
            }
            if(CorrectItems == stash.craftedItem.Recipe.Count)
            {
                Debug.Log("inside of providing the item are we here ever?");
                //run code to supply the crafted item here 
                ItemDefinition newCraftedItem =stash.craftedItem;

                if(IsServer)
                {
                    tempPawn.playerInventory.Additem(newCraftedItem);
                    stash.CraftItemRPC(newCraftedItem.itemId);
                }
                else
                {
                    tempPawn.playerInventory.Additem(newCraftedItem);
                    stash.CraftItemRPC(newCraftedItem.itemId);
                }

                ReturnItems();
            }
        }
       
        UpdateList();
        CorrectItems = 0;
        return;
    }

   public void DropDownValueChanged(Dropdown dd)
    {
        stash.craftedItem = stash.CraftableItems[dd.value];
        Debug.Log($"item to be crafted {stash.craftedItem}");
    }



    public void ReturnItems()
    {
        stash.ReturnItemsPerform();

        if(!IsServer)
        {
            stash.ReturnItemRPC();
        }

    }
    public void TakeItem(int i)
    {
        Debug.Log("Index where item should've been taken from " + i);
        PrisonerPawn tempPawn = (PrisonerPawn)_player.myPawn;
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
                stash.TakeItemRPC(_container.ItemsInContainer[i].instanceId);
            }
            else
            {
                stash.TakeItemRPC(_container.ItemsInContainer[i].instanceId);
                tempPawn.playerInventory.Additem(_container.TakeItem(i));
            }
        }
    }

    public void AddItem(int i)
    {
        PrisonerPawn tempPawn = (PrisonerPawn)_player.myPawn;
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
                stash.AddItemRPC(tempPawn.playerInventory.ItemsInContainer[i].instanceId);
                tempPawn.playerInventory.TakeItem(i);
            }
            else
            {
                stash.AddItemRPC(tempPawn.playerInventory.ItemsInContainer[i].instanceId);
                _container.Additem(tempPawn.playerInventory.TakeItem(i));
            }

        }
    }

    public void UpdateList()
    {
        int i = 0;
        PrisonerPawn tempPawn = (PrisonerPawn)_player.myPawn;
        PlayerInv = tempPawn.playerInventory.ItemsInContainer;

        //Containerbuttons.Clear();
        //Playerbuttons.Clear();

        for (int buttons = 0; buttons < 6; buttons++)
        {
            if (buttons < 3)
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

    public void dropDownList()
    {
        List<string> itemNames = new List<string>();


        foreach(ItemDefinition item in stash.CraftableItems)
        {
            itemNames.Add(item.ItemName);
        }

        dropDown.AddOptions(itemNames);
    }
   

}
