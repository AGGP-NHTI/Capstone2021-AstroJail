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
            Done();
            temp.ObjectUsing = null;
            UsingPlayer = null;
            return true;
        }
        else
        {
            MoveDoor();
            Done();
            UsingPlayer = null;
            temp.ObjectUsing = null;
        }
        return true;
    }
    public void MoveDoor()
    {
        if(IsServer)
        {
            GetComponent<Animator>().Play("doors", 0);
        }
        else
        {
            MoveDoorServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void MoveDoorServerRpc()
    {
        GetComponent<Animator>().Play("doors", 0);
    }

}
