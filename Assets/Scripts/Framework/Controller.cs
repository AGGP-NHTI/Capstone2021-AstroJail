using MLAPI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : NetworkedBehaviour
{
    public Pawn myPawn;

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
