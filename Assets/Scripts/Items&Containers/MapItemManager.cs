﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapItemManager : MonoBehaviour
{
    public List<Containers> containerList;
    public List<ItemDefinition> itemList;
    void Start()
    {
        foreach (Containers box in containerList)
        {
            foreach (ItemDefinition item in box.startingItems)
            {
                //create a copy of the item 
                ItemDefinition newItem = Instantiate(item);

                //set item copy container to this container
                newItem.startingLocation = box;
                newItem.itemId = itemList.Count;

                itemList.Add(newItem);
                //put item into container (items in container)
                box.Additem(newItem);

            }

            foreach (ItemDefinition item in box.craftableItems)
            {
                item.itemId = itemList.Count;
                item.startingLocation = box;
                itemList.Add(item);
            }



        }
        // Debug Testing, 
        if (true)
        {
            Debug.Log(" ===== ");
            Debug.Log("ItemList Details");
            foreach (ItemDefinition item in itemList)
            {
                Debug.Log(item.itemId + ": " + item.ItemName + " in " + item.startingLocation.gameObject.name ); 
            }

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
