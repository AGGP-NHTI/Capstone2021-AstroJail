using UnityEngine;
using MLAPI.Messaging;
using MLAPI;
using System.ComponentModel;
using System.Reflection.Emit;
using TMPro;
public class GuardItemStash : MapInteractable
{
    public bool IsPanelActive = false;
    public GameObject labelObject;
    public ItemDefinition ItemToGive;
    public GameObject HudReference;
    public GuardPawn PawnReff = null;

    public void Start()
    {
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
        PawnReff = other.gameObject.GetComponentInParent<GuardPawn>();
        
        if (PawnReff)
        {
            if (PawnReff.playerType == PlayerType.Prisoner)
            {
                return;
            }
            if (PawnReff.control)
            {
                if (PawnReff.control.IsLocalPlayer)
                {
                    PawnReff.Interactables.Add(this);
                    Label.SetActive(true);
                }
            }
        }
    }

    public override bool OnUse(PlayerController user)
    {
        if(PawnReff.playerInventory.itemCount > 0)
        {
            PawnReff.playerInventory.ItemsInContainer.Clear();
            PawnReff.playerInventory.Additem(ItemToGive);
        } 
        else
        {
            PawnReff.playerInventory.Additem(ItemToGive);
        }
         IsPanelActive = true;
        UsingPlayer = null;
        Done();
        PawnReff.ObjectUsing = null;

        return true;
    }

}
