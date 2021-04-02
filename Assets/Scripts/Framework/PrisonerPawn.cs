using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrisonerPawn : PlayerPawn
{
    public override void Initialize()
    {
        playerType = PlayerType.Prisoner;
    }

}
