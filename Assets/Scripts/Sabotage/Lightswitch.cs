using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using TMPro;
using MLAPI.Messaging;
public class Lightswitch : MapInteractable
{
    public bool lightState = true;
    public float lightIntensity;
    public float lightRange;
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
                    if(lightState == true && p.playerType == PlayerType.Prisoner)
                    {
                        p.Interactables.Add(this);
                        Label.SetActive(true);
                    }
                    else if(lightState == false && p.playerType == PlayerType.Guard)
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
        if(lightState == true)
        {
            Label.GetComponent<TextMeshPro>().text = "Press E to sabotage lights";
            itemReqLabel.GetComponent<TextMeshPro>().text = "Requires: Tazer";
        }
        else
        {
            Label.GetComponent<TextMeshPro>().text = "Press E to fix lights";
            itemReqLabel.GetComponent<TextMeshPro>().text = "Requires: Wrench";
        }
    }

    public override bool OnUse(PlayerController user)
    {
        PlayerPawn player = (PlayerPawn)user.myPawn;

        if(player.playerType == PlayerType.Prisoner)
        {
            foreach(ItemDefinition item in player.playerInventory.ItemsInContainer)
            {
                if(item.itemId == 6)
                {
                    TurnLightsOffServerRpc(true, item.instanceId);
                    player.playerInventory.ItemsInContainer.Remove(item);
                    Label.SetActive(false);
                    break;
                }
            }
        }
        else
        {
            if (player.playerInventory.ItemsInContainer[0].itemId == 21)
            {
                TurnLightsOffServerRpc(false);
                player.playerInventory.ItemsInContainer.Clear();
                Label.SetActive(false);
            }
        }

        player.EndInteract();
        return true;
    }

    [ServerRpc(RequireOwnership = false)]
    public void TurnLightsOffServerRpc(bool lightsOff, int itemID = -1)
    {
        List<Light> lights = new List<Light>(GameObject.FindObjectsOfType<Light>());

        if(lightsOff)
        {
            foreach(Light l in lights)
            {
                if(l.transform.parent.tag == gameObject.tag)
                {
                    l.intensity = 0f;
                    l.range = 0;
                }
            }
            lightState = false;
        }
        else
        {
            foreach (Light l in lights)
            {
                if (l.transform.parent.tag == gameObject.tag)
                {
                    l.intensity = 2.5f;
                    l.range = 25;
                }
            }
            lightState = true;
        }
        if(itemID != -1)
        {
            if(!MapItemManager.Instance.itemList[itemID].isCrafted)
            {
                MapItemManager.Instance.itemList[itemID].startingLocation.Additem(MapItemManager.Instance.itemList[itemID]);
            }
        }
        TurnLightsOffClientRpc(lightsOff);
    }

    [ClientRpc]
    public void TurnLightsOffClientRpc(bool lightsOff, int itemID = -1)
    {
        if (IsServer) { return; }

        List<Light> lights = new List<Light>(GameObject.FindObjectsOfType<Light>());

        if (lightsOff)
        {
            foreach (Light l in lights)
            {
                if (l.transform.parent.tag == gameObject.tag)
                {
                    l.intensity = 0f;
                    l.range = 0;
                }
            }
            lightState = false;
        }
        else
        {
            foreach (Light l in lights)
            {
                if (l.transform.parent.tag == gameObject.tag)
                {
                    l.intensity = 2.5f;
                    l.range = 25;
                }
            }
            lightState = true;
        }
    }
}
