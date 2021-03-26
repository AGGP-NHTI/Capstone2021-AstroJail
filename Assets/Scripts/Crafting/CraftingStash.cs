using MLAPI.Prototyping;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI.Messaging;
using MLAPI;
using System.ComponentModel;
using System.Reflection.Emit;
using TMPro;

public class CraftingStash : MapInteractable
{

    //rename this to Generic Stash 
    //this goes on a mapInteractable that a player can go up to and grab or place an item into 


    public GameObject HUDPanelToAttach;
    public bool IsPanelActive = false;
    public GameObject labelObject;
    Containers container;
    public GameObject HudReference;
    public ItemDefinition craftedItem;
    public List<ItemDefinition> CraftableItems;
   

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
        PlayerPawn p = other.gameObject.GetComponentInParent<PlayerPawn>();
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
      
        if (HUDPanelToAttach.GetComponent<CraftingHUD>())
        {
            HudReference = Instantiate(HUDPanelToAttach);
            HudReference.GetComponent<CraftingHUD>()._container = container;
            HudReference.GetComponent<CraftingHUD>()._player = UsingPlayer;
            HudReference.GetComponent<CraftingHUD>().stash = this;
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
    public void CraftItemRPC(int i)
    {
        InvokeServerRpc(Server_CraftItem, i);
    }

    public void ReturnItemRPC()
    {
        InvokeServerRpc(Server_ReturnItems);
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

    //---------------- Crafted items ------------------//

    [ClientRPC]

    public void Client_CraftItem(int id)
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

    [ServerRPC(RequireOwnership = false)]

    public void Server_CraftItem(int id)
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
        InvokeClientRpcOnEveryone(Client_CraftItem, id);
;    }


    [ClientRPC]
    public void Client_ReturnItem(List<int> items)
    {
        if (IsServer) return;

    }

    [ServerRPC(RequireOwnership =false)]
    public void Server_ReturnItems()
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
