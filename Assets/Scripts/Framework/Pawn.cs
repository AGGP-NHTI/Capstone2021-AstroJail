using MLAPI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pawn : Actor
{
    public Controller control;
    public GameObject CameraControl;
    public PlayerType playerType = PlayerType.Prisoner;
    public void Possessed(Controller c)
    {
        control = c;
        
        OnPossessed();

        NetworkedObject NetObj = gameObject.GetComponent<NetworkedObject>();
        if(NetObj)
        {
            Debug.Log("Changed net Owner on Pawn");
            //NetObj.ChangeOwnership(c.OwnerClientId);
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
    public virtual void Jump(bool s)
    {
       
    }

    public virtual void Interact(bool e)
    {

    }
    public virtual void Close(bool escape)
    {

    }
    public virtual void EndInteract()
    {

    }
    public virtual void Fire1()
    {

    }
}
