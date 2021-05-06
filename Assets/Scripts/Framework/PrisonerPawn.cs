using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Connection;
using MLAPI.Messaging;

public class PrisonerPawn : PlayerPawn
{
    private Animator anim;

    public bool isBeingSearched = false;
    public bool canOpenCell = true;
    public float timer;
    public float cellTime = 20;
    public override void Initialize()
    {
        playerType = PlayerType.Prisoner;
    }

    public override void Update()
    {
        base.Update();
        if(canOpenCell==false)
        {
            timer += Time.deltaTime;
            if(timer >=cellTime)
            {
                canOpenCell = true;
                timer = 0;
            }
        }

        if (control)
        {
            if (control.IsLocalPlayer)
            {
                SetAnimatorParams();
            }
        }


    }

    public override void Start()
    {
        base.Start();
        anim = gameObject.GetComponentInChildren<Animator>();

    }

    public override void Jump(bool s)
    {
        if(isBeingSearched)
        {
            return;
        }
        base.Jump(s);
        if(s)
        {
            anim.SetBool("jump", true);
        }
        else
        {
            anim.SetBool("jump", false);
        }
    }

    public override void Move(float horizontal, float vertical)
    {
        if (isBeingSearched)
        {
            return;
        }
        base.Move(horizontal, vertical);
    }
    public void ReturnItems()
    {
        ReturnItemsServerRpc(this.OwnerClientId);
    }

    public override void SetCamPitch(float value)
    {
        if (isBeingSearched)
        {
            return;
        }
        base.SetCamPitch(value);
    }
    public override void Interact(bool e)
    {
        if (isBeingSearched)
        {
            return;
        }
        base.Interact(e);
    }

    private void SetAnimatorParams()
    {
        Vector3 localVelocity = transform.InverseTransformVector(rb.velocity);
        anim.SetFloat("moveSpeedZ", localVelocity.z);
        anim.SetFloat("moveSpeedX", localVelocity.x);
    }

    [ServerRpc(RequireOwnership = false)]
    public void ReturnItemsServerRpc(ulong clientId)
    {  
        ClientRpcParams CRP = new ClientRpcParams();
        ulong[] targetClientID = new ulong[1];
        targetClientID[0] = clientId;
        CRP.Send.TargetClientIds = targetClientID;
        ReturnItemsPerform();
       
        ReturnItemsClientRpc(CRP);
    }
    [ClientRpc]
    public void ReturnItemsClientRpc(ClientRpcParams CRP = default)
    {
        canOpenCell = false;
        transform.position = SpawnPoints.Instance.randomJailPoint();
        playerInventory.ItemsInContainer.Clear();
    }

    public void ReturnItemsPerform()
    {

        foreach (ItemDefinition item in playerInventory.ItemsInContainer)
        {
            if (item.isCrafted)
            {

            }
            else
            {
                item.startingLocation.Additem(item);
            }
           

        }
        playerInventory.ItemsInContainer.Clear();

    }

}
