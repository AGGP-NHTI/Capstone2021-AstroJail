using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using TMPro;
using MLAPI.Messaging;
public class ElevatorSabotage : MapInteractable
{
    public GameObject itemReqLabel;
    public bool elevatorDisabled;
    public PlayerVent elevator1;
    public PlayerVent elevator2;

    public override void OnTriggerEnter(Collider other)
    {
        PlayerPawn p = other.gameObject.GetComponentInParent<PlayerPawn>();
        if (p)
        {
            if (p.control)
            {
                if (p.control.IsLocalPlayer)
                {
                    if (elevatorDisabled == false && p.playerType == PlayerType.Prisoner)
                    {
                        p.Interactables.Add(this);
                        Label.SetActive(true);
                    }
                    else if (elevatorDisabled == true && p.playerType == PlayerType.Guard)
                    {
                        p.Interactables.Add(this);
                        Label.SetActive(true);
                    }
                }
            }
        }
    }

    void Update()
    {
        if (Label.activeSelf)
        {
            Label.transform.rotation = Quaternion.LookRotation(Label.transform.position - Camera.main.transform.position);
        }
        if (elevatorDisabled == false)
        {
            Label.GetComponent<TextMeshPro>().text = "Press E to sabotage elevator";
            itemReqLabel.GetComponent<TextMeshPro>().text = "Requires: Wirecutters";
        }
        else
        {
            Label.GetComponent<TextMeshPro>().text = "Press E to fix lights";
            itemReqLabel.GetComponent<TextMeshPro>().text = "Requires: Wires";
        }
    }

    public override bool OnUse(PlayerController user)
    {
        PlayerPawn player = (PlayerPawn)user.myPawn;

        if (player.playerType == PlayerType.Prisoner)
        {
            foreach (ItemDefinition item in player.playerInventory.ItemsInContainer)
            {
                if (item.itemId == 24)
                {
                    SabotageElevatorServerRpc(true, item.instanceId);
                    player.playerInventory.ItemsInContainer.Remove(item);
                    Label.SetActive(false);
                    break;
                }
            }
        }
        else
        {
            if (player.playerInventory.ItemsInContainer[0].itemId == 12)
            {
                SabotageElevatorServerRpc(false);
                player.playerInventory.ItemsInContainer.Clear();
                Label.SetActive(false);
            }
        }

        player.EndInteract();
        return true;
    }

    [ServerRpc(RequireOwnership = false)]
    public void SabotageElevatorServerRpc(bool sabotage, int itemID = -1)
    {
        if (itemID != -1)
        {
            if (!MapItemManager.Instance.itemList[itemID].isCrafted)
            {
                MapItemManager.Instance.itemList[itemID].startingLocation.Additem(MapItemManager.Instance.itemList[itemID]);
            }
        }

        SabotageElevatorClientRpc(sabotage);
    }

    [ClientRpc]
    public void SabotageElevatorClientRpc(bool sabotage, int itemID = -1)
    {
        if (IsServer) { return; }

    }
}