using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrisonerPawn : PlayerPawn
{
    public override void Initilize()
    {
        playerType = PlayerType.Prisoner;
    }

}
