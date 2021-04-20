using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrisonerPawn : PlayerPawn
{
    public bool isBeingSearched = false;
    public override void Initialize()
    {
        playerType = PlayerType.Prisoner;
    }
    public override void Jump(bool s)
    {
        if(isBeingSearched)
        {
            return;
        }
        base.Jump(s);
    }

    public override void Move(float horizontal, float vertical)
    {
        if (isBeingSearched)
        {
            return;
        }
        base.Move(horizontal, vertical);
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

}
