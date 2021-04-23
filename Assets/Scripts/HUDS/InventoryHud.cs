using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryHud : MonoBehaviour
{
    private List<ItemDefinition> itemsInInventory;
    public List<Image> inventorySlots;
    private Pawn me;
    void Start()
    {
        me = gameObject.GetComponentInParent<Pawn>();
    }

    // Update is called once per frame
    void Update()
    {
        if(me.playerType == PlayerType.Prisoner)
        {
            populateInventorySlotsPrisoner();
        }
        else
        {
            populateInventorySlotsGuard();
        }

    }

    private void populateInventorySlotsPrisoner()
    {
        PrisonerPawn temp = (PrisonerPawn)me;
        itemsInInventory = temp.playerInventory.ItemsInContainer;

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

    private void populateInventorySlotsGuard()
    {
        GuardPawn temp = (GuardPawn)me;
        itemsInInventory = temp.playerInventory.ItemsInContainer;

        int index = 0;
        for (int totalIndex = 0; totalIndex < inventorySlots.Count; totalIndex++)
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
