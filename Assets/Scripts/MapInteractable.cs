using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MLAPI.Messaging;
using MLAPI;

public class MapInteractable : NetworkedBehaviour
{
    public GameObject Label;
    protected PlayerController UsingPlayer;



    public void OnTriggerEnter(Collider other)
    {
        PlayerPawn p = other.gameObject.GetComponentInParent<PlayerPawn>();
        if (p)
        {
            if (p.control)
            {
                if (p.control.IsLocalPlayer)
                {
                    p.Interactables.Add(this);
                    Label.SetActive(true);
                    Debug.Log("Detected Player");

                }
            }
        }
    }
    public void OnTriggerExit(Collider other)
    {
        PlayerPawn p = other.gameObject.GetComponentInParent<PlayerPawn>();
        if (p)
        {
            if (p.control)
            {
                if (p.control.IsLocalPlayer)
                {
                    p.Interactables.Remove(this);
                    Label.SetActive(false);
                    Debug.Log("Player Left Vicinity");

                }
            }
        }
    }

    public bool Use(PlayerController user)
    {
        if (UsingPlayer)
        {
            return false;
        }

        if(IsServer)
        {
            InvokeClientRpcOnEveryone(Client_InteractableStartUse, user);
        }
        else
        {
            InvokeServerRpc(Server_InteractableStartUse, user);
        }


        if (OnUse(user))
        {
            return true;
        }
        else
        {
            Done();
            return false;
        }

    }

    public bool Done()
    {
        //Always called from PlayerPawn
        if (IsServer)
        {
            InvokeClientRpcOnEveryone(Client_InteractableStopUse);
        }
        else
        {
            InvokeServerRpc(Server_InteractableStopUse);
        }
        return OnDone();
    }

    /// <summary>
    /// The OnUse method return value is the deciding factor for whether or not the PlayerPawn movement will be locked, Return false to keep free movement, return true to lock the player down.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public virtual bool OnUse(PlayerController user)
    {
        return false;
    }

    public virtual bool OnDone()
    {
        return true;
    }

    public bool IsInUse()
    {
        //should return true if set 
        //should return false if null
        //using unity hack if causes prob will fix later
        return UsingPlayer;
    }

    [ClientRPC]
    public void Client_InteractableStartUse(PlayerController user)
    {
        UsingPlayer = user;
    }

    [ClientRPC]
    public void Client_InteractableStopUse()
    {
        UsingPlayer = null;
    }

    [ServerRPC(RequireOwnership = false)]
    public void Server_InteractableStartUse(PlayerController user)
    {
        UsingPlayer = user;
        InvokeClientRpcOnEveryone(Client_InteractableStartUse, user);
    }
    [ServerRPC(RequireOwnership = false)]
    public void Server_InteractableStopUse()
    {
        UsingPlayer = null;
        InvokeClientRpcOnEveryone(Client_InteractableStopUse);
    }


}
