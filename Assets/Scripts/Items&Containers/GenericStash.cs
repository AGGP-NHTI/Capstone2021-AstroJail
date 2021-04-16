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

    public override void OnTriggerEnter(Collider other)
    {      
        PrisonerPawn p = other.gameObject.GetComponentInParent<PrisonerPawn>();
        if (p)
        {
            if(p.playerType == PlayerType.Guard)
            {
                return;
            }
            if (p.control)
            {
                if (p.control.IsLocalPlayer)
                {
                    p.Interactables.Add(this);
                    Label.SetActive(true);
                }
            }
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


    [ServerRpc(RequireOwnership = false)]
    public void TakeItemServerRpc(int itemID)
    {
        ItemDefinition itemDef = MapItemManager.Instance.itemList[itemID];

        container.TakeItem(itemDef);

        ClientRpcParams targetClient = new ClientRpcParams();
        ulong[] target = new ulong[1];
        target[0] = UsingPlayer.OwnerClientId;
        targetClient.Send.TargetClientIds = target;

        UpdateItemsForClientRpc(targetClient);
    }

    [ClientRpc]
    public void UpdateItemsForClientRpc(ClientRpcParams targetClient = default)
    {
        HudReference.GetComponent<ContainerHUD>().UpdateList();
    }


}

//This Script can look at whats in containers
//This script will also call from TakeItem and AddItem
//enum that checks for guard or prisoner
