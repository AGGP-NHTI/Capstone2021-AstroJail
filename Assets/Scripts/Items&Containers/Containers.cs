using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Containers : MonoBehaviour
{

    public int MaxItems;
    public List<ItemDefinition> startingItems;
    public List<ItemDefinition> craftableItems;
    public List<ItemDefinition> ItemsInContainer;


    public int itemCount
    {
        get { return ItemsInContainer.Count; }
    }


    //*************************
    // TO DO:
    // need variable localPlayer 
    // when set the container will be a part of a player 
    // ASSUMED: will be class type variable and will be able to use null to indicate not part of player
    //*************************



    //i need this or failure 
    public PlayerPawn thePlayer;

    void Start()
    {  
        // This got moved to Map Item Manager
        // This is to control who gets processed first
        /*
        foreach (ItemDefinition item in startingItems)
        {
            //create a copy of the item 
            ItemDefinition newItem = Instantiate(item);

            //set item copy container to this container
            newItem.startingLocation = this;

            //put item into container (items in container)
            this.Additem(newItem);

        }
        */
    }
    private void Update()
    {
        debugTake();
    }

    public void debugTake()
    {
      
    }

    //this may not be the version/method we want to use :subject to change
    public ItemDefinition TakeItem(ItemDefinition item)
    {     
        //TO DO :   
        //validate that the item given is in the list 
        //need to be careful about exact item instance vs. just an item type
       
        //verify that the item is still there
        ItemsInContainer.Remove(item);
        //return null when you cannot remove the item asked 
        return null;
    }
    public ItemDefinition TakeItem(int itemAt)
    {
       
        if(itemAt>(ItemsInContainer.Count-1))
        {
            Debug.LogError($"no stop get some help{itemAt} Is not there");
            return null;
        }
        
        ItemDefinition temp = ItemsInContainer[itemAt];

        ItemsInContainer.RemoveAt(itemAt);



        //grab this return value to take item and place into player inventory 
        Debug.Log($"you removed {temp} from the container");
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
    
}
