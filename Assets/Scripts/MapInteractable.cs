using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MLAPI.Messaging;
using MLAPI;
using MLAPI.Connection;

public class MapInteractable : NetworkBehaviour
{
    public GameObject Label;
    [SerializeField]
    protected PlayerController UsingPlayer;
    [SerializeField]
    protected bool beingUsed;

    public virtual void OnTriggerEnter(Collider other)
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
        if (beingUsed)
        {
            return false;
        }
        UsingPlayer = user;
        InteractableStartUseServerRpc(user.OwnerClientId);
        return true;
    }

    public bool Done()
    {
        //Always called from PlayerPawn
        if (IsServer)
        {
            InteractableStopUseClientRpc();
        }
        else
        {
            InteractableStopUseServerRpc();
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

    [ClientRpc]
    public void InteractableStartUseClientRpc()
    {
        if (IsServer) return;
        beingUsed = true;
    }

    [ClientRpc]
    public void InteractableStopUseClientRpc()
    {
        beingUsed = false;
    }

    [ClientRpc]
    public void InteractableStartOnClientRpc(ulong user, ClientRpcParams clientID = default)
    {
        Debug.Log("I should've opened the stash hud for my client");
        foreach (NetworkClient NC in NetworkManager.Singleton.ConnectedClientsList)
        {
            if (NC.ClientId == user)
            {
                Debug.Log("we called OnUse()");
                OnUse(NC.PlayerObject.GetComponent<PlayerController>());
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void InteractableStartUseServerRpc(ulong user)
    {
        foreach (NetworkClient NC in NetworkManager.Singleton.ConnectedClientsList)
        {
            if (NC.ClientId == user)
            {
                UsingPlayer = NC.PlayerObject.gameObject.GetComponent<PlayerController>();
                beingUsed = true;
                ClientRpcParams targetClient = new ClientRpcParams();
                ulong[] targetClientId = new ulong[1];
                targetClientId[0] = user;
                targetClient.Send.TargetClientIds = targetClientId;


                InteractableStartOnClientRpc(user, targetClient);
                InteractableStartUseClientRpc();
            }
        }

    }
    [ServerRpc(RequireOwnership = false)]
    public void InteractableStopUseServerRpc()
    {
        UsingPlayer = null;
        beingUsed = false;
        InteractableStopUseClientRpc();
    }


}
