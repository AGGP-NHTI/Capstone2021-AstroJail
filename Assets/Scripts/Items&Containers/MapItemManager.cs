using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapItemManager : MonoBehaviour
{
    private static MapItemManager _instance;
    
    public static MapItemManager Instance { get { return _instance; } }

    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

    }

    public List<Containers> containerList;
    public List<ItemDefinition> itemList;
    public List<ItemDefinition> craftedItems;
    public List<ItemDefinition> everyItem;
    public Containers myBox;


    void Start()
    {
        foreach (Containers box in containerList)
        {
            myBox = box;
            box.containerID = containerList.IndexOf(box);
            foreach (ItemDefinition item in box.startingItems)
            {
                //create a copy of the item 
                ItemDefinition newItem = Instantiate(item);

                //set item copy container to this container
                newItem.startingLocation = box;
                newItem.instanceId = itemList.Count;

                itemList.Add(newItem);
                //put item into container (items in container)
                box.Additem(newItem);

            }

            /*
            foreach (ItemDefinition item in box.craftableItems)
            {
                item.itemId = itemList.Count;
                item.startingLocation = box;
                itemList.Add(item);
            }*/

        }
        // Debug Testing, 
        if (false)
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
