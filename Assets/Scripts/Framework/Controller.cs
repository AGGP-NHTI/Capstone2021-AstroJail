using MLAPI;
using MLAPI.Messaging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : NetworkBehaviour
{
    public Pawn myPawn;
    public ClientRpcParams myClientParams;

    public virtual void Start()
    {
        myClientParams = new ClientRpcParams();
        ulong[] clientID = new ulong[1];
        clientID[0] = OwnerClientId;
        myClientParams.Send.TargetClientIds = clientID;
        
    }
    public void PossessPawn(GameObject go)
    {
        Pawn x = go.GetComponent<Pawn>();

        if(x)
        {
            PossessPawn(x);
        }
        else
        {
            Debug.Log(go.name + "Pawn Missing");
        }
    }

    public void PossessPawn(Pawn x)
    {
        if(myPawn)
        {
            myPawn.UnPossess();
        }

        myPawn = x;
        x.Possessed(this);
    }

}
