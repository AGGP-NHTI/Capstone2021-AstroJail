using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class CellDoors : MapInteractable
{

    PrisonerPawn temp;
    public GameObject label,label2;

    public GameObject theDoor;
    public float timer;
    public Vector3 difference;
    public Vector3 point;
    public float percent;
    public float openedTimer;
    public bool isOpen = false;

    public bool openedDoor = false;
    public float height;
    public Vector3 originPos;


    // Start is called before the first frame update
    void Start()
    {
        label.SetActive(false);
        originPos = theDoor.transform.position;
        point = new Vector3(theDoor.transform.position.x, height, theDoor.transform.position.z);
        difference = point - originPos;

    }

    // Update is called once per frame
    void Update()
    {

        if (openedDoor == true)
        {
            MoveDoor();
        }

    }
    public override void OnTriggerEnter(Collider other)
    {
        PrisonerPawn p = other.gameObject.GetComponentInParent<PrisonerPawn>();
        temp = p;
        if (p)
        {
            if (p.playerType == PlayerType.Guard)
            {
                return;
            }
            if (p.control)
            {
                if (p.control.IsLocalPlayer)
                {
                    p.Interactables.Add(this);
                    Label.SetActive(true);
                    Debug.Log("Detected Player");
                }
            }
        }
    }
    public override bool OnUse(PlayerController user)
    {
        if (temp.canOpenCell == false)
        {
            UsingPlayer = null;
            return true;
        }
        else
        {
            openedDoor = true;
            UsingPlayer = null;
            beingUsed = false;
            temp.ObjectUsing = null;

        }
        return true;
    }
    public void MoveDoor()
    {
        MoveDoorServerRpc();
     
    }

    [ServerRpc(RequireOwnership = false)]
    public void MoveDoorServerRpc()
    {
        if (isOpen == false)
        {
            if (timer <= 1.5)
            {
                timer += Time.deltaTime;
                percent = timer / 1.5f;
                theDoor.transform.position = originPos + (difference * percent);

            }
            else { isOpen = true; timer = 0; }


        }
        else if (isOpen == true)
        {
            if (openedTimer <= 3)
            {
                openedTimer += Time.deltaTime;
            }
            else
            {
                if (timer <= 1.5)
                {
                    timer += Time.deltaTime;
                    percent = timer / 1.5f;
                    theDoor.transform.position = point - (difference * percent);

                }
                else
                {
                    timer = 0;
                    isOpen = false;
                    openedDoor = false;
                    openedTimer = 0;
                }

            }

        }
    }

}
