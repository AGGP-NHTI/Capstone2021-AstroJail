using UnityEngine;
using MLAPI.Messaging;
using MLAPI;
using System.ComponentModel;
using System.Reflection.Emit;
using TMPro;
public class FixSabotage : MapInteractable
{
    public bool IsPanelActive = false;
    public GameObject labelObject;
    public ItemDefinition ItemToDestroy;
    public bool hasBeenFixed = false;
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
        if (PawnReff.playerInventory.ItemsInContainer[0] == ItemToDestroy)
        {
            PawnReff.playerInventory.ItemsInContainer.Clear();
            hasBeenFixed = true;
        } 
        IsPanelActive = true;
        PawnReff.ObjectUsing = null;
        return true;
    }

}

