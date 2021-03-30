using MLAPI.Messaging;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class GuardPawn : PlayerPawn
{
    
    Camera theCam;
    Ray locateRay;
    public float searchDistance = 10f;
    public PrisonerPawn FoundPlayer = null;
    public PrisonerPawn searchedPlayer = null;
   

    //Properties


    public override void Initilize()
    {
        playerType = PlayerType.Guard;
        theCam = CameraControl.GetComponent<Camera>();
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
        locateRay.direction = theCam.transform.forward ;
        //this might need to be moved to a better starting position
        locateRay.origin = gameObject.transform.position;
        
        RaycastHit hitInfo;

        if (Physics.Raycast(locateRay, out hitInfo, searchDistance))
        {
            FoundPlayer = hitInfo.collider.gameObject.GetComponentInParent<PrisonerPawn>();
           
            
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
            if(FoundPlayer && !searchedPlayer)
            {
                searchedPlayer = FoundPlayer;
                this.lockMovement = true;
                searchedPlayer.lockMovement = true;

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
    

    public override void Close(bool escape)
    {

        if (escape && ObjectUsing)
        {
            EndInteract();
        }
        if(escape && searchedPlayer)
        {
            this.lockMovement = false;
            searchedPlayer.lockMovement = false;
            searchedPlayer = null;
            EndInteract();
        }
       

    }

}
