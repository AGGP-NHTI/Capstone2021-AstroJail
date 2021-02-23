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
        /*
        PlayerPawn Player = other.gameObject.GetComponent<PlayerPawn>();
        if (Player)
        {
            Player.Interactables.Add(this);
            Label.SetActive(true);
            Debug.Log("Detected Player");
        }
        */
        //may need to do more code to do operations on local client only 
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
        /*
        PlayerPawn Player = other.gameObject.GetComponent<PlayerPawn>();
        if (Player)
        {
            Player.Interactables.Remove(this);
            Label.SetActive(false);
            Debug.Log("Player Left Vicinity");
        }
        */

        //may need to do more code to do operations on local client only 

    }

    public bool Use(PlayerController user)
    {
        if(UsingPlayer)
        {
            return false;
        }
        UsingPlayer = user;
        return OnUse(user);
    }

    public bool Done()
    {
        UsingPlayer = null;
        return OnDone();
    }

    public virtual bool OnUse(PlayerController user)
    {
        return true;
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

    private void ClientTriggerEnter(Collider other)
    {
        PlayerPawn Player = other.gameObject.GetComponent<PlayerPawn>();
        if (Player)
        {
            Player.Interactables.Add(this);
            Label.SetActive(true);
            Debug.Log("Detected Player");
        }
    }
    [ClientRPC]
    private void ClientTriggerExit(Collider other)
    {
        PlayerPawn Player = other.gameObject.GetComponent<PlayerPawn>();
        if (Player)
        {
            Player.Interactables.Remove(this);
            Label.SetActive(false);
            Debug.Log("Player Left Vicinity");
        }
    }


}
