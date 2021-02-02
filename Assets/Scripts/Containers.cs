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
        foreach(ItemDefinition item in startingItems)
        {
            
            //create a copy of the item 
            //set item copy container to this container
            //put item into container (items in container)
        }
    }

    public ItemDefinition TakeItem(ItemDefinition item)
    {
        return null;
    }
    
    public bool Additem(ItemDefinition item)
    {
        //return false when the item cannot be added
        return true;
    }
    
}
