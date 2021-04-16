using MLAPI.Prototyping;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI.Messaging;
using MLAPI;
using System.ComponentModel;
using System.Reflection.Emit;
using TMPro;
public class ItemTransferVent : MapInteractable
{

    //rename this to Generic Stash 
    //this goes on a mapInteractable that a player can go up to and grab or place an item into 


    public GameObject HUDPanelToAttach;
    public bool IsPanelActive = false;
    public GameObject labelObject;
    Containers container;
    public GameObject HudReference;
    public ItemTransferVent TransferDestination;


    //order of operations that are happening as things are being interacted with 
    //-------------------------------------------------------------------------
    //this is across multiple scripts and both client and server
    //1.) local: player walks into the interactable trigger 
    //2.) player now knows that it can interact with object 
    //3.) local player hits e to interact
    //4.) jump request from local client to the server || Client to server rpc
    //5.) validate that the interactable can be used
    //6a.) if in use: return back the requesting user that they cannot use it || Server to client rpc 
    //     on client this ends the interaction with object 
    //6b.) if not in use: set up so that that player is using it by set usingPlayer to requested player 
    //6c.) tell local requesting player they can use interactable || server to client rpc 

    //7.) local player now knows they are using an object 
    //8.) for the container it now needs to add the panel to the local player 
    //9.) panel will run interactions between containers and what not 
    //9a.) this is an abstraction and these interactions will require thei rown client and server interactions 

    //10.) local player hits escape 
    //11.) done gets called locally 
    //11a.) on client: panel gets removed. using object gets set to null. 

    //12.) done called on server
    //12a.) On server: UsingPlayer gets set to null.
    //13.) This ends the interaction 


    // this set of operations above dont include accounting for 
    // controlling a label on top of an object to indicate its current state to the other players 
    // part of this is going to be done locally and part of it will be done pushing information to the server down 

    //on use : it should add a hud panel to the users hud 
    //the logic for the panel should be on a hud panel script 
    //the panel will have a reference to this object 




    //for matt: work on the panel 
    //pass items over the network - will look at on friday with prof. walek 
    //when up to container panel will not know everything yet 
    //do the same for the player as well 
    //do all interactions with items on the server 
    //this prevents hacking on client side 

    public void Start()
    {
        container = gameObject.GetComponent<Containers>();
        container.itemUpdateCallback = ItemsUpdated;
        labelObject.SetActive(false);
    }
    private void Update()
    {
        if (labelObject.activeSelf)
        {
            labelObject.transform.rotation = Quaternion.LookRotation(labelObject.transform.position - Camera.main.transform.position);
        }
    }

    public void ItemsUpdated()
    {
        //this creates the itemhud and gives the items in container

        HudReference = Instantiate(HUDPanelToAttach);
        HudReference.GetComponent<ItemTransferHUD>()._container = container;
        HudReference.GetComponent<ItemTransferHUD>()._player = UsingPlayer;
        HudReference.GetComponent<ItemTransferHUD>().stash = this;

    }

    public void TransferItem()
    {
        TransferItemServerRpc(container.ItemsInContainer[0].instanceId, TransferDestination.container.containerID);
    }

    public override bool OnUse(PlayerController user)
    {
        IsPanelActive = true;
        labelObject.GetComponent<TextMeshPro>().text = "In Use";

        InUseServerRpc();
        container.ServerRequestItems(UsingPlayer.OwnerClientId);

        return true;
    }

    public override bool OnDone()
    {
        labelObject.GetComponent<TextMeshPro>().text = "Press E to Interact";
        UsingPlayer = null;

        if (HudReference)
        {
            Debug.Log(HudReference);
            Destroy(HudReference);
            Debug.Log(HudReference);
        }

        StopUseServerRpc();
        return true;
    }

    public void AddItemRPC(int i)
    {
        AddItemServerRpc(i);
    }

    public void TakeItemRPC(int i)
    {
        TakeItemServerRpc(i);
    }



    //-------------Start interact/End Interact RPCs--------------//
    [ClientRpc]
    public void InUseClientRpc()
    {
        if (IsServer) return;
        labelObject.GetComponent<TextMeshPro>().text = "In Use";
    }
    [ClientRpc]
    public void StopUseClientRpc()
    {
        if (IsServer) return;
        labelObject.GetComponent<TextMeshPro>().text = "Press E to Interact";
    }
    [ServerRpc(RequireOwnership = false)]
    public void InUseServerRpc()
    {
        labelObject.GetComponent<TextMeshPro>().text = "In Use";
        InUseClientRpc();
    }
    [ServerRpc(RequireOwnership = false)]
    public void StopUseServerRpc()
    {
        labelObject.GetComponent<TextMeshPro>().text = "Press E to Interact";
        StopUseClientRpc();
    }

    //-------------Taking/Adding item to container RPCs--------------//
    [ClientRpc]
    public void TakeItemClientRpc(int itemID)
    {
        ItemDefinition itemDef = MapItemManager.Instance.itemList[itemID];

        container.TakeItem(itemDef);
    }
    [ClientRpc]
    public void AddItemClientRpc(int itemID)
    {
        ItemDefinition itemDef = MapItemManager.Instance.itemList[itemID];

        container.Additem(itemDef);
    }

    [ServerRpc(RequireOwnership = false)]
    public void TakeItemServerRpc(int itemID)
    {
        ItemDefinition itemDef = MapItemManager.Instance.itemList[itemID];

        container.TakeItem(itemDef);
    }
    [ServerRpc(RequireOwnership = false)]
    public void AddItemServerRpc(int itemID)
    {
        ItemDefinition itemDef = MapItemManager.Instance.itemList[itemID];

        container.Additem(itemDef);
    }

    //---------------Transfer Item RPC----------------//

    [ClientRpc]
    public void TransferItemClientRpc(int instanceID, int containerID)
    {
        if(IsServer)
        {
            return;
        }
        Containers destination = MapItemManager.Instance.containerList[containerID];
        ItemDefinition item = MapItemManager.Instance.itemList[instanceID];

        destination.Additem(item);
        this.container.TakeItem(item);
    }

    [ServerRpc(RequireOwnership = false)]
    public void TransferItemServerRpc(int instanceID, int containerID)
    {
        Containers destination = MapItemManager.Instance.containerList[containerID];
        ItemDefinition item = MapItemManager.Instance.itemList[instanceID];

        
        if( destination.ItemsInContainer.Count > 0)//checks if there is already an item in the destination container
        {
            return;
        }
        

        destination.Additem(item);
        this.container.TakeItem(item);

        TransferItemClientRpc(instanceID, containerID);
    }
}

//This Script can look at whats in containers
//This script will also call from TakeItem and AddItem
//enum that checks for guard or prisoner
