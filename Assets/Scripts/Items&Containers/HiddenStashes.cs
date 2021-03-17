using MLAPI.Prototyping;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI.Messaging;
using MLAPI;
using System.ComponentModel;
using System.Reflection.Emit;
using TMPro;

public class HiddenStashes : MapInteractable
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
        if (HUDPanelToAttach.GetComponent<HiddenStashHud>())
        {
            HudReference = Instantiate(HUDPanelToAttach);
            HudReference.GetComponent<HiddenStashHud>()._container = container;
            HudReference.GetComponent<HiddenStashHud>()._player = UsingPlayer;
            HudReference.GetComponent<HiddenStashHud>().stash = this;
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

        if (HudReference)
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

    public void AddItemRPC(int i)
    {
        InvokeServerRpc(Server_AddItem, i);
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
    [ClientRPC]
    public void Client_AddItem(int itemID)
    {
        ItemDefinition itemDef = MapItemManager.Instance.itemList[itemID];

        container.Additem(itemDef);
    }

    [ServerRPC(RequireOwnership = false)]
    public void Server_TakeItem(int itemID)
    {
        ItemDefinition itemDef = MapItemManager.Instance.itemList[itemID];

        container.TakeItem(itemDef);
    }
    [ServerRPC(RequireOwnership = false)]
    public void Server_AddItem(int itemID)
    {
        ItemDefinition itemDef = MapItemManager.Instance.itemList[itemID];

        container.Additem(itemDef);
    }


}

//This Script can look at whats in containers
//This script will also call from TakeItem and AddItem
//enum that checks for guard or prisoner