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
        if(UsingPlayer)
        {
            return false;
        }
        UsingPlayer = user;

        if(OnUse(user))
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
        UsingPlayer = null;
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



}
