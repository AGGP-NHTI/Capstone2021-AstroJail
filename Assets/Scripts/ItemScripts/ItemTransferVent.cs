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
    public GameObject HUDPanelToAttach;
    public bool IsPanelActive = false;
    public GameObject labelObject;
    Containers container;
    public GameObject HudReference;
    public ItemTransferVent TransferDestination;


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

        ClientRpcParams targetClient = new ClientRpcParams();
        ulong[] target = new ulong[1];
        target[0] = UsingPlayer.OwnerClientId;
        targetClient.Send.TargetClientIds = target;

        UpdateItemsForClientRpc(targetClient);
    }

    [ServerRpc(RequireOwnership = false)]
    public void AddItemServerRpc(int itemID)
    {
        ItemDefinition itemDef = MapItemManager.Instance.itemList[itemID];

        container.Additem(itemDef);

        ClientRpcParams targetClient = new ClientRpcParams();
        ulong[] target = new ulong[1];
        target[0] = UsingPlayer.OwnerClientId;
        targetClient.Send.TargetClientIds = target;

        UpdateItemsForClientRpc(targetClient);
    }

    [ClientRpc]
    public void UpdateItemsForClientRpc(ClientRpcParams targetClient = default)
    {
        HudReference.GetComponent<ItemTransferHUD>().UpdateList();
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
