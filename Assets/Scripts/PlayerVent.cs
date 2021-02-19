using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVent : MapInteractable
{
    public GameObject endPoint;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Label.activeSelf)
        {
            Label.transform.rotation = Quaternion.LookRotation(Label.transform.position - Camera.main.transform.position);
        }
    }

    public override bool OnUse(PlayerController user)
    {
        UsingPlayer = null;
        PlayerPawn tempPawn = (PlayerPawn)user.myPawn;
        tempPawn.transform.position = endPoint.transform.position;
        tempPawn.ObjectUsing = null;
        
        return true;
    }
}
