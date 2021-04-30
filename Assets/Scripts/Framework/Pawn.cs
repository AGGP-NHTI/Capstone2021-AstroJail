using MLAPI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Pawn : Actor
{
    public TextMeshPro NamePlate;
    public Controller control;
    public GameObject CameraControl;
    public GameObject playerUI;
    public GameObject playerModel;
    public PlayerType playerType = PlayerType.Prisoner;
    public void Possessed(Controller c)
    {
        control = c;
        
        OnPossessed();

        NetworkObject NetObj = gameObject.GetComponent<NetworkObject>();
        if(NetObj)
        {
            //NetObj.ChangeOwnership(c.OwnerClientId);
        }
        else
        {
        }

    }
    public virtual void Update()
    {
        if(NamePlate.gameObject.activeSelf)
        {
            if(Camera.main)
            {
                NamePlate.transform.rotation = Quaternion.LookRotation(NamePlate.transform.position - Camera.main.transform.position);
            }
        }
    }
    public virtual void OnPossessed()
    {

    }

    public virtual void UnPossess()
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
    public virtual void Search(bool q)
    {

    }

    public virtual void FlashLight(bool f)
    {

    }

    public virtual void Fire1()
    {

    }
    public virtual void OpenPlayerMenu()
    {

    }
}
