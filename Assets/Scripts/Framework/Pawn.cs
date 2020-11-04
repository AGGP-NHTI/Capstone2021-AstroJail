using MLAPI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Actor
{
    Controller control;

    public void Possessed(Controller c)
    {
        control = c;

        OnPossessed();

        NetworkedObject NetObj = gameObject.GetComponent<NetworkedObject>();
        if(NetObj)
        {
            Debug.Log("Changed net Owner on Pawn");
            NetObj.ChangeOwnership(c.OwnerClientId);
        }
        else
        {
            Debug.Log("No NetObj found");
        }

    }

    public void OnPossessed()
    {

    }

    public void UnPossess()
    {

    }

    public virtual void RotatePlayer(float MouseX)
    {

    }
    public virtual void Move(float h, float v)
    {

    }

    public virtual void Fire1()
    {

    }


}
