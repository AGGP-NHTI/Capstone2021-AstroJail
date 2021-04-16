using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class Containers : NetworkBehaviour
{
    public delegate void ContainerCallback();
    public ContainerCallback itemUpdateCallback;
    public int containerID;
    public int MaxItems;
    public List<ItemDefinition> startingItems;
    public List<ItemDefinition> ItemsInContainer;
    

    public int[] GetItemArray()
    {
        int[] temp = new int[6];
        for(int i = 0; i < temp.Length;i++)
        {
            temp[i] = -1;
        }
        for(int i = 0;i < ItemsInContainer.Count;i++)
        {
            temp[i] = ItemsInContainer[i].instanceId;
        }
        return temp;
    }

    public int itemCount
    {
        get { return ItemsInContainer.Count; }
    }

    //this may not be the version/method we want to use :subject to change
    public ItemDefinition TakeItem(ItemDefinition item)
    {     
        foreach(ItemDefinition items in ItemsInContainer)
        {
            if(items.instanceId == item.instanceId)
            {
                ItemsInContainer.Remove(items);
                return items;
            }
        }
        //return null when you cannot remove the item asked 
        return null;
    }

    public ItemDefinition TakeItem(int itemAt)
    {
       
        if( itemAt > (ItemsInContainer.Count-1))
        {
            Debug.LogError($"no stop get some help{itemAt} Is not there");
            return null;
        }
        
        ItemDefinition temp = ItemsInContainer[itemAt];

        ItemsInContainer.RemoveAt(itemAt);


        return temp;
    }
    
    public bool Additem(ItemDefinition item)
    {
        //return false when the item cannot be added
        if (ItemsInContainer.Count>=MaxItems)
        {
            return false;
        }
        ItemsInContainer.Add(item);
        return true;
    }


    public void ServerRequestItems(ulong id)
    {
        Debug.Log("were w called");
        RequestItemsServerRpc(id);
    }

    public void GuardRequestItems(ulong prisonerID, ulong guardID)
    {
        RequestItemsFromClientServerRpc(prisonerID, guardID);
    }

    /// <summary>
    /// This request is from the client to the server, asks the server for a list of items in this container
    /// </summary>
    /// <param name="clientID"></param>
    [ServerRpc(RequireOwnership = false)]
    public void RequestItemsServerRpc(ulong clientID)
    {
        ClientRpcParams CRP = new ClientRpcParams();
        ulong[] targetClientID = new ulong[1];
        targetClientID[0] = clientID;
        CRP.Send.TargetClientIds = targetClientID;


        ItemListUpdateClientRpc(GetItemArray(), CRP);
    }

    [ServerRpc(RequireOwnership = false)]
    public void RequestItemsFromClientServerRpc(ulong prisonerID, ulong guardID)
    {
        ClientRpcParams CRP = new ClientRpcParams();
        ulong[] targetClientID = new ulong[1];
        targetClientID[0] = prisonerID;
        CRP.Send.TargetClientIds = targetClientID;


        GiveServerItemsClientRpc(prisonerID, guardID, CRP);
    }

    [ServerRpc(RequireOwnership = false)]
    public void GiveItemsToClientServerRpc(int[] instanceIDs, ulong guardID)
    {
        ClientRpcParams CRP = new ClientRpcParams();
        ulong[] targetClientID = new ulong[1];
        targetClientID[0] = guardID;
        CRP.Send.TargetClientIds = targetClientID;


        ItemListUpdateClientRpc(instanceIDs, CRP);
    }

    [ClientRpc]
    public void GiveServerItemsClientRpc(ulong prisonerID, ulong guardID, ClientRpcParams CRP = default)
    {
        GiveItemsToClientServerRpc(GetItemArray(), guardID);
    }

    [ClientRpc]
    public void ItemListUpdateClientRpc(int[] itemList, ClientRpcParams CRP = default )
    {
        Debug.Log("In the Client_ItemListUpdate with list size of: " + itemList.Length);
        foreach (int i in itemList)
        {
            Debug.Log(i);
        }
            ItemsInContainer.Clear();

        foreach(int i in itemList)
        {
            if(i != -1)
            {
                Debug.Log("This is the item we should be adding to the container: " + MapItemManager.Instance.itemList[i]);
                ItemsInContainer.Add(MapItemManager.Instance.itemList[i]);
            }
        }

        if(itemUpdateCallback != null)
        {
            itemUpdateCallback.Invoke();
        }
    }

}
