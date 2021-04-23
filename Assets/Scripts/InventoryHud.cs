using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryHud : MonoBehaviour
{
    private List<ItemDefinition> itemsInInventory;
    public List<Image> inventorySlots;
    private PrisonerPawn me;
    void Start()
    {
        me = gameObject.GetComponentInParent<PrisonerPawn>();
    }

    // Update is called once per frame
    void Update()
    {
        populateInventorySlots();
    }

    private void populateInventorySlots()
    {
        itemsInInventory = me.playerInventory.ItemsInContainer;

        int index = 0;
        for(int totalIndex = 0; totalIndex < inventorySlots.Count; totalIndex++)
        {
            if (index < itemsInInventory.Count)
            {
                inventorySlots[totalIndex].sprite = itemsInInventory[index].imageArt;
                inventorySlots[totalIndex].color = Color.white;
            }
            else
            {
                inventorySlots[totalIndex].sprite = null;
                inventorySlots[totalIndex].color = Color.clear;
            }

            index++;
        }
    }
}
