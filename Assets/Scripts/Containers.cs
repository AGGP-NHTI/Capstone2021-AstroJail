using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Containers : MonoBehaviour
{
    public List<ItemDefinition> startingItems;
    public List<ItemDefinition> ItemsInContainer;
    public int MaxItems = 5;
    // Start is called before the first frame update
    void Start()
    {
        foreach (ItemDefinition item in startingItems)
        {

            //create a copy of the item 
            ItemDefinition newItem = (ItemDefinition)ScriptableObject.CreateInstance(item.GetType());

            //set item copy container to this container
            newItem.startingLocation = this;

            //put item into container (items in container)
            this.Additem(newItem);


        }
    }

    public ItemDefinition TakeItem(ItemDefinition item)
    {
        return null;
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
    
}
