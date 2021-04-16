using MLAPI.Prototyping;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI.Messaging;
using MLAPI;
using System.ComponentModel;
using System.Reflection.Emit;
using TMPro;

public class WinConditionStash : MapInteractable
{
    public GameObject HUDPanelToAttach;
    public bool IsPanelActive = false;
    public GameObject labelObject;
    Containers container;
    public GameObject HudReference;
    public List<ItemDefinition> winConditon;


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

        if (HUDPanelToAttach.GetComponent<WinConditionHud>())
        {
            HudReference = Instantiate(HUDPanelToAttach);
            HudReference.GetComponent<WinConditionHud>()._container = container;
            HudReference.GetComponent<WinConditionHud>()._player = UsingPlayer;
            HudReference.GetComponent<WinConditionHud>().stash = this;
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
            InUseServerRpc();
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
    public void CraftItemRPC(int i)
    {
        CraftItemServerRpc(i);
    }

    public void ReturnItemRPC()
    {
        ReturnItemsServerRpc();
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

    //---------------- Crafted items ------------------//

    [ClientRpc]

    public void CraftItemClientRpc(int id)
    {
        if (IsServer) return;
        foreach (ItemDefinition item in MapItemManager.Instance.everyItem)
        {
            if (id == item.itemId)
            {
                ItemDefinition tempItem = item;
                tempItem.instanceId = MapItemManager.Instance.itemList.Count;
                Debug.Log("Our crafted items instance ID is: " + tempItem.instanceId);
                MapItemManager.Instance.itemList.Add(tempItem);
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]

    public void CraftItemServerRpc(int id)
    {
        foreach (ItemDefinition item in MapItemManager.Instance.everyItem)
        {
            if (id == item.itemId)
            {
                ItemDefinition tempItem = item;
                tempItem.instanceId = MapItemManager.Instance.itemList.Count;
                MapItemManager.Instance.itemList.Add(tempItem);
            }
        }
        CraftItemClientRpc(id);
        ;
    }


    [ServerRpc(RequireOwnership = false)]
    public void ReturnItemsServerRpc()
    {
        ReturnItemsPerform();
        //known issue: if other client using containeras where items are being returned to
        //client will not see the updated list
        //next time client intereacts with container they will see updated list  

    }




    public void ReturnItemsPerform()
    {

        foreach (ItemDefinition item in container.ItemsInContainer)
        {
            item.startingLocation.Additem(item);

        }
        container.ItemsInContainer.Clear();

    }

}
