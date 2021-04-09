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

        playerType = PlayerType.Guard;
        theCam = Camera.main;
        locateRay = new Ray();
    }
    public override void Update()
    {
        //may be something in pawn that we need to do 
        base.Update();
        FindPrisoners();
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

                
                 searchedPlayer.playerInventory.GuardRequestItems(searchedPlayer.OwnerClientId,OwnerClientId);
                            
                


                //for me to put in here 
                //pull up player inventory hud. send found player to hud same as stashed
                //move logic to hud script



                // we have done this locally on guard client machine 
                // we now need to replicate what we have done here acrossed the network 
                // insync with the player that is being searched 
                // we may have to tell the prisoner where they need to be standing
                // they need a hud to show up saying they are being searched 
                // get there inventory then show the guards search hud with that information 


                // search begins 
                // Guard sends server rpc and it states which player and their location 
                // in server rpc 
                // 1.) sends client rpc (prisoner inventory) to guard with prisoner inventory 
                // 1.a) guard can open hud element with prisoner inventory information
                // 2.) sends client rpc (search begins) to prisoner to inform them being searched and a location
                // 2.a) prisoner can open hud element and places themselves at location




                // the search interaction ends when the guard
                // either
                // A.) ends the search without picking an item from inventory
                // B.) follows through on search

                // if A, then we need to call end interaction on prisoner(across network) and guard
                // if B, guard wins/ prisoner wins interaction

                // if guard wins
                // report interaction to all players 
                // then send items back to starting stash
                // teleport player to cell 
                // release both players and remove huds(ends interaction)

                // if Prisoner wins 
                // report interaction to all players 
                // Guard enters sleep/cooldown state
                // release both players and remove huds(ends interaction)  


            }
        }

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
        if (escape && searchedPlayer)
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
        searchedPlayer = null;
        EndInteract();
    }

    [ServerRPC(RequireOwnership = false)]
    public void Server_RequestItems(ulong playerID)
    {
        int[] test;
        PrisonerPawn temp = null;
        foreach (NetworkedClient nc in NetworkingManager.Singleton.ConnectedClientsList)
        {
            if (nc.ClientId == playerID)
            {
                temp = (PrisonerPawn)nc.PlayerObject.GetComponent<PlayerController>().myPawn;
            }
        }

    InvokeClientRpcOnClient(Client_RequestItems,playerID);

       
    }

    [ClientRPC]
    public void Client_RequestItems()
    {
        PrisonerPawn temp = null;
        List<int> itemIDs = new List<int>();
        foreach (PlayerController pc in GameObject.FindObjectsOfType<PlayerController>())
        {
            if(pc.myController)
            {
                temp = (PrisonerPawn)pc.myPawn;
            }
        }

        foreach(ItemDefinition id in temp.playerInventory.ItemsInContainer)
        {
            itemIDs.Add(id.instanceId);
        }
        int[] items = itemIDs.ToArray();

     
    }
}
