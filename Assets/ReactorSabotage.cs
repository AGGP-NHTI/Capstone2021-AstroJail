using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using TMPro;
using MLAPI.Messaging;
public class ReactorSabotage : MapInteractable
{
    public bool reactorMeltdown = false;
    public GameObject itemReqLabel;

    public override void OnTriggerEnter(Collider other)
    {
        PlayerPawn p = other.gameObject.GetComponentInParent<PlayerPawn>();
        if (p)
        {
            if (p.control)
            {
                if (p.control.IsLocalPlayer)
                {
                    if (reactorMeltdown == false && p.playerType == PlayerType.Prisoner)
                    {
                        p.Interactables.Add(this);
                        Label.SetActive(true);
                    }
                    else if (reactorMeltdown == true && p.playerType == PlayerType.Guard)
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
        if (reactorMeltdown == false)
        {
            Label.GetComponent<TextMeshPro>().text = "Press E to smack the reactor";
            itemReqLabel.GetComponent<TextMeshPro>().text = "Requires: Tazer Nunchucks";
        }
        else
        {
            Label.GetComponent<TextMeshPro>().text = "Press E to fix lights";
            itemReqLabel.GetComponent<TextMeshPro>().text = "Requires: Toolbox";
        }
    }

    public override bool OnUse(PlayerController user)
    {
        PlayerPawn player = (PlayerPawn)user.myPawn;

        if (player.playerType == PlayerType.Prisoner)
        {
            foreach (ItemDefinition item in player.playerInventory.ItemsInContainer)
            {
                if (item.itemId == 22)
                {
                    ReactorMeltdownServerRpc(true, true,item.instanceId);
                    player.playerInventory.ItemsInContainer.Remove(item);
                    Label.SetActive(false);
                    break;
                }
            }
        }
        else
        {
            if (player.playerInventory.ItemsInContainer[0].itemId == 23)
            {
                ReactorMeltdownServerRpc(false, false);
                player.playerInventory.ItemsInContainer.Clear();
                Label.SetActive(false);
            }
        }

        player.EndInteract();
        return true;
    }

    [ServerRpc(RequireOwnership = false)]
    public void ReactorMeltdownServerRpc(bool meltdown, bool lightsOff, int itemID = -1)
    {
        List<Light> lights = new List<Light>(GameObject.FindObjectsOfType<Light>());

        if (lightsOff)
        {
            foreach (Light l in lights)
            {
                l.GetComponent<Animator>().SetBool("reactorMeltdown", true);
            }
        }
        else
        {
            foreach (Light l in lights)
            {
                if (l.GetComponentInParent<PlayerPawn>())
                { }
                else
                {
                    l.GetComponent<Animator>().SetBool("reactorMeltdown", false);
                }
            }
        }


        if (itemID != -1)
        {
            if (!MapItemManager.Instance.itemList[itemID].isCrafted)
            {
                MapItemManager.Instance.itemList[itemID].startingLocation.Additem(MapItemManager.Instance.itemList[itemID]);
            }
        }
        ReactorMeltdownClientRpc(meltdown ,lightsOff);
    }

    [ClientRpc]
    public void ReactorMeltdownClientRpc(bool meltdown ,bool lightsOff)
    {
        if (IsServer) { return; }

        List<Light> lights = new List<Light>(GameObject.FindObjectsOfType<Light>());

        if (lightsOff)
        {
            foreach (Light l in lights)
            {
                l.GetComponent<Animator>().SetBool("reactorMeltdown", true);
            }
        }
        else
        {
            foreach (Light l in lights)
            {
                if (l.GetComponentInParent<PlayerPawn>())
                { }
                else
                {
                    l.GetComponent<Animator>().SetBool("reactorMeltdown", false);
                }
            }
        }
    }
}