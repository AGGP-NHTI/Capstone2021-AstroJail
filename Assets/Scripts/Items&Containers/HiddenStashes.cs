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
    public override void OnTriggerEnter(Collider other)
    {
        PrisonerPawn p = other.gameObject.GetComponentInParent<PrisonerPawn>();
        if (p)
        {
            if (p.playerType == PlayerType.Guard)
            {
                return;
            }
            if (p.control)
            {
                if (p.control.IsLocalPlayer)
                {
                    p.Interactables.Add(this);
                    Label.SetActive(true);
                    Debug.Log("Detected Player");
                }
            }
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
            InUseClientRpc();
        }
        else
        {
            InUseClientRpc();
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
            StopUseClientRpc();
        }
        else
        {
            StopUseServerRpc();
        }
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

        labelObject.GetComponent<TextMeshPro>().text = "In Use";
    }
    [ClientRpc]
    public void StopUseClientRpc()
    {
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


}

//This Script can look at whats in containers
//This script will also call from TakeItem and AddItem
//enum that checks for guard or prisoner
