using MLAPI.Prototyping;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI.Messaging;
using MLAPI;
using System.ComponentModel;
using System.Reflection.Emit;
using TMPro;
public class GenericStash : MapInteractable
{

    //rename this to Generic Stash 
    //this goes on a mapInteractable that a player can go up to and grab or place an item into 

   
    public GameObject HUDPanelToAttach;
    public bool IsPanelActive = false;
    public GameObject labelObject;
    Containers container;
    public GameObject HudReference;
    

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
        if (HUDPanelToAttach.GetComponent<ContainerHUD>())
        {
            HudReference = Instantiate(HUDPanelToAttach);
            HudReference.GetComponent<ContainerHUD>()._container = container;
            HudReference.GetComponent<ContainerHUD>()._player = UsingPlayer;
            HudReference.GetComponent<ContainerHUD>().stash = this;
        }
        else if (HUDPanelToAttach.GetComponent<CraftingHUD>())
        {
            HudReference = Instantiate(HUDPanelToAttach);
            HudReference.GetComponent<CraftingHUD>()._container = container;
            HudReference.GetComponent<CraftingHUD>()._player = UsingPlayer;
        }
    }

    public override bool OnUse(PlayerController user)
    {
        IsPanelActive = true;
        labelObject.GetComponent<TextMeshPro>().text = "In Use";

        if (IsServer)
        {
            InvokeClientRpcOnEveryone(Client_InUse);
        }
        else
        {
            InvokeServerRpc(Server_InUse);
        }

        Debug.Log(user);
        container.ServerRequestItems(UsingPlayer.OwnerClientId);

        return true;
    }

    public override bool OnDone()
    {   
        labelObject.GetComponent<TextMeshPro>().text = "Press E to Interact";
        UsingPlayer = null;

        if(HudReference)
        {
            Debug.Log(HudReference);
            Destroy(HudReference);
            Debug.Log(HudReference);
        }
        if (IsServer)
        {
            InvokeClientRpcOnEveryone(Client_StopUse);
        }
        else
        {
            InvokeServerRpc(Server_StopUse);
        }
        return true;
    }

   
    public void TakeItemRPC(int i)
    {
            InvokeServerRpc(Server_TakeItem, i);
    }



    //-------------Start interact/End Interact RPCs--------------//
    [ClientRPC]
    public void Client_InUse()
    {
       
        labelObject.GetComponent<TextMeshPro>().text = "In Use";
    }
    [ClientRPC]
    public void Client_StopUse()
    {
        labelObject.GetComponent<TextMeshPro>().text = "Press E to Interact";
    }
    [ServerRPC(RequireOwnership = false)]
    public void Server_InUse()
    {
        labelObject.GetComponent<TextMeshPro>().text = "In Use";
        InvokeClientRpcOnEveryone(Client_InUse);
    }
    [ServerRPC(RequireOwnership = false)]
    public void Server_StopUse()
    {
        labelObject.GetComponent<TextMeshPro>().text = "Press E to Interact";
        InvokeClientRpcOnEveryone(Client_StopUse);
    }

    //-------------Taking/Adding item to container RPCs--------------//
    [ClientRPC]
    public void Client_TakeItem(int itemID)
    {
        ItemDefinition itemDef = MapItemManager.Instance.itemList[itemID];

        container.TakeItem(itemDef);
    }
    

    [ServerRPC(RequireOwnership = false)]
    public void Server_TakeItem(int itemID)
    {
        ItemDefinition itemDef = MapItemManager.Instance.itemList[itemID];

        container.TakeItem(itemDef);
    }
   

}

//This Script can look at whats in containers
//This script will also call from TakeItem and AddItem
//enum that checks for guard or prisoner