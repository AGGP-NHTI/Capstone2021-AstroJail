using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerVent : MapInteractable
{
    public GameObject endPoint;
    private float CooldownTimer;
    public float maxCooldown;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(CooldownTimer > 0)
        {
            Label.GetComponent<TextMeshPro>().text = CooldownTimer.ToString("0");
            OnCooldown();
        }
        else
        {
            Label.GetComponent<TextMeshPro>().text = "Press E to Interact";

        }
        if (Label.activeSelf)
        {
            Label.transform.rotation = Quaternion.LookRotation(Label.transform.position - Camera.main.transform.position);
        }

    }

    public override bool OnUse(PlayerController user)
    {
        PlayerPawn tempPawn = (PlayerPawn)user.myPawn;
        if (CooldownTimer > 0)
        {
            UsingPlayer = null;
            tempPawn.ObjectUsing = null;
            return false;
        }
        UsingPlayer = null;

        tempPawn.transform.position = endPoint.transform.position;
        tempPawn.ObjectUsing = null;
        CooldownTimer = maxCooldown;
        
        return true;
    }

    public void OnCooldown()
    {
        CooldownTimer -= Time.deltaTime;
    }
}
