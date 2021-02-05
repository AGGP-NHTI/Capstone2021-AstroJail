using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//KNOWN ISSUE :
//WILL NEED UPDATING UI AND OTHER ENTITIES WHEN CONTENTS CHANGE 
//THINK LIKE A MINECRAFT CHEST IN MULTIPLAYER. YOU SHOULD BE ABLE TO SEE WHEN CONTENTS CHANGE
public class Containers : MonoBehaviour
{

    public int MaxItems = 5;
    public List<ItemDefinition> startingItems;
    public List<ItemDefinition> ItemsInContainer;
    int itemAtMe;
    
    // Start is called before the first frame update
    void Start()
    {
        foreach (ItemDefinition item in startingItems)
        {
            //create a copy of the item 
            // ItemDefinition newItem = (ItemDefinition)ScriptableObject.CreateInstance(item.GetType());
            ItemDefinition newItem = Instantiate(item);

            //set item copy container to this container
            newItem.startingLocation = this;

            //put item into container (items in container)
            this.Additem(newItem);

        }
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            TakeItem(1);
        }
    }

    /*
    //this is for debugging 
    private void Update()
    {
         if(Input.GetKeyDown(KeyCode.Space))
        {
            foreach (ItemDefinition item in ItemsInContainer)
            {
                Debug.Log(item.itemId + ": " + item.ItemName);
            }
        }       
    }
    */


    public ItemDefinition TakeItem(ItemDefinition item)
    {     
        //TO DO :        
       
        //verify that the item is still there
        ItemsInContainer.Remove(item);
        //return null when you cannot remove the item asked 
        return null;
    }
    public ItemDefinition TakeItem(int itemAt)
    {
        //this may not be the version/method we want to use :subject to change
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
