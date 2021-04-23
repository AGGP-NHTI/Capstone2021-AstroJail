using MLAPI.Messaging;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using MLAPI;
using MLAPI.Connection;

public class GuardPawn : PlayerPawn
{
    public GameObject HUDPanelToAttach;
    public GameObject HudReference;
    Containers container;
    Camera theCam;
    Ray locateRay;
    public float searchDistance = 10f;
    public PrisonerPawn FoundPlayer = null;
    public PrisonerPawn searchedPlayer = null;
    public List<int> toRecieve;
    public GameObject rayPoint;
    public float timer = 0;
    public float failTime = 5;
    public bool failedSearch = false;

    //Properties

    /*
    public override void Initialize()
    {
        playerType = PlayerType.Guard;
        theCam = CameraControl.GetComponent<Camera>();
        locateRay = new Ray();
    }
    */

    public override void Start()
    {
        base.Start();
        timer = 0;
        playerType = PlayerType.Guard;
        theCam = Camera.main;
        locateRay = new Ray();
    }
    public override void Update()
    {
        if (failedSearch)
        {
            Debug.Log("timer: " + timer);
            timer += Time.deltaTime;
            if (timer >= failTime)
            {
                
                failedSearch = false;
                timer = 0;
            }

        }

        //may be something in pawn that we need to do 
        base.Update();
        FindPrisoners();
    }

    //movement overrides 
    //
    public override void Jump(bool s)
    {
        if (failedSearch)
        {
            return;
        }
        base.Jump(s);
    }

    public override void Move(float horizontal, float vertical)
    {
        if (failedSearch)
        {
            return;
        }
        base.Move(horizontal, vertical);
    }

    public override void SetCamPitch(float value)
    {
        if (failedSearch)
        {
            return;
        }
        base.SetCamPitch(value);
    }


    public void FindPrisoners()
    {
        FoundPlayer = null;
        locateRay.direction = rayPoint.transform.forward;
        //this might need to be moved to a better starting position
        locateRay.origin = rayPoint.transform.position;
        Debug.DrawRay(locateRay.origin, locateRay.direction * searchDistance, Color.red);
        RaycastHit hitInfo;

        if (Physics.Raycast(locateRay, out hitInfo, searchDistance))
        {
            Debug.Log(hitInfo.collider.gameObject.name);
            if (hitInfo.collider.gameObject.GetComponentInParent<PrisonerPawn>())
            {
                FoundPlayer = hitInfo.collider.gameObject.GetComponentInParent<PrisonerPawn>();

            }

        }

    }
    public override void Interact(bool e)
    {
        if(failedSearch)
        {
            return;
        }
        if (e && !ObjectUsing && (Interactables.Count != 0))
        {
            Debug.Log($"pressed e: {e} ");
            if (Interactables[0].IsInUse())
            {
                Debug.Log("object is in use");

            }
            else
            {
                if (Interactables[0].Use((PlayerController)control))
                {
                    ObjectUsing = Interactables[0];
                }
            }
        }
    }
    public override void Search(bool q)
    {
        if (q)
        {
            if (FoundPlayer && !searchedPlayer)
            {

                Debug.Log("in found player");
                searchedPlayer = FoundPlayer;
                searchedPlayer.playerInventory.itemUpdateCallback = ItemsUpdated;
                this.lockMovement = true;
                searchedPlayer.lockMovement = true;


                searchedPlayer.playerInventory.GuardRequestItems(searchedPlayer.OwnerClientId, OwnerClientId);
                BeginSearchPrisonerServerRpc(searchedPlayer.OwnerClientId);





            }
        }

    }
    public void FailedSearch()
    {
        Debug.Log("inside of failed search function");
        failedSearch = true;

    }

    public void ItemsUpdated()
    {
        //this creates the itemhud and gives the items in container
        if (HUDPanelToAttach.GetComponent<SearchPlayerHud>())
        {
            Debug.Log("are we here in items update");
            HudReference = Instantiate(HUDPanelToAttach);
            HudReference.GetComponent<SearchPlayerHud>()._container = searchedPlayer.playerInventory;
            HudReference.GetComponent<SearchPlayerHud>()._player = (PlayerController)control;
            //HudReference.GetComponent<SearchPlayerHud>().stash = this;
        }
    }

    public override void Close(bool escape)
    {

        if (escape && ObjectUsing)
        {
            EndInteract();
        }
        if (escape && searchedPlayer && failedSearch == false)
        {
            DoneSearching();
        }
    }

    public void DoneSearching()
    {

        Debug.Log(HudReference);
        Destroy(HudReference);
        Debug.Log(HudReference);

        this.lockMovement = false;
        searchedPlayer.lockMovement = false;
        EndSearchPrisonerServerRpc(searchedPlayer.OwnerClientId);
        searchedPlayer = null;
        
        EndInteract();
    }

  [ServerRpc(RequireOwnership =false)]
  public void BeginSearchPrisonerServerRpc(ulong playerID)
    {
         ClientRpcParams CRP = new ClientRpcParams();
        ulong[] targetClientID = new ulong[1];
        targetClientID[0] = playerID;
        CRP.Send.TargetClientIds = targetClientID;

        BeginSearchPrisonerClientRpc(CRP);
    }

    [ClientRpc]
    public void BeginSearchPrisonerClientRpc(ClientRpcParams clientRpcParams = default)
    {
        PrisonerPawn temp = null;

        foreach (PlayerController pc in GameObject.FindObjectsOfType<PlayerController>())
        {
            if (pc.myController)
            {
                temp = (PrisonerPawn)pc.myPawn;
            }
        }

  
        temp.isBeingSearched = true;


    }
    [ServerRpc(RequireOwnership = false)]
    public void EndSearchPrisonerServerRpc(ulong playerID)
    {

        ClientRpcParams CRP = new ClientRpcParams();
        ulong[] targetClientID = new ulong[1];
        targetClientID[0] = playerID;
        CRP.Send.TargetClientIds = targetClientID;


        EndSearchPrisonerClientRpc(CRP);
    }


    [ClientRpc]
    public void EndSearchPrisonerClientRpc(ClientRpcParams clientRpcParams = default)
    {
        PrisonerPawn temp = null;

        foreach (PlayerController pc in GameObject.FindObjectsOfType<PlayerController>())
        {
            if (pc.myController)
            {
                temp = (PrisonerPawn)pc.myPawn;
            }
        }


        temp.isBeingSearched = false;


    }
}
