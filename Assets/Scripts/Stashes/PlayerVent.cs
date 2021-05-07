using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerVent : MapInteractable
{
    public GameObject endPoint;
    private float CooldownTimer;
    public float maxCooldown;
    public bool sabotaged = false;
    public bool isVent;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (CooldownTimer > 0)
        {
            Label.GetComponent<TextMeshPro>().text = CooldownTimer.ToString("0");
            OnCooldown();
        }
        else
        {
            Label.GetComponent<TextMeshPro>().text = "Press E to crawl through";
        }
        if (Label.activeSelf)
        {
            Label.transform.rotation = Quaternion.LookRotation(Label.transform.position - Camera.main.transform.position);
        }
    }

    public override bool OnUse(PlayerController user)
    {
        if (isVent)
        {
            PrisonerPawn tempPawn = (PrisonerPawn)user.myPawn;
            if (tempPawn.playerType == PlayerType.Prisoner)
            {

                if (sabotaged)
                {
                    tempPawn.EndInteract();
                }

                if (CooldownTimer > 0)
                {
                    tempPawn.EndInteract();
                    UsingPlayer = null;
                    return false;
                }
                tempPawn.transform.position = endPoint.transform.position;
                CooldownTimer = maxCooldown;

                tempPawn.EndInteract();
                UsingPlayer = null;
                return false;
            }
            else
            {
                tempPawn.EndInteract();
                UsingPlayer = null;
                return false;
            }
        }
        else
        {
            PlayerPawn tempPawn = (PlayerPawn)user.myPawn;

            if (sabotaged)
            {
                tempPawn.EndInteract();
            }
            if (CooldownTimer > 0)
            {
                tempPawn.EndInteract();
                UsingPlayer = null;
                return false;
            }
            tempPawn.transform.position = endPoint.transform.position;
            CooldownTimer = maxCooldown;

            tempPawn.EndInteract();
            UsingPlayer = null;
            return false;
        }

    }
    public void OnCooldown()
    {
        CooldownTimer -= Time.deltaTime;
    }
}
