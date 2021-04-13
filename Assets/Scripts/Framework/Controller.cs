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
        myClientParams.Send.TargetClientIds[0] = OwnerClientId;
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
