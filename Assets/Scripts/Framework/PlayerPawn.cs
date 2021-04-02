using MLAPI.Messaging;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerPawn : Pawn
{

    public int maxInventory;
    protected Containers _PlayerInventory;
 
    public GameObject projSpawn;
    public GameObject projPrefab;
    public float mouseSensitivity = 15;
    public float moveRate = 7.5f;
    public float rotationRate = 90;
    public float pitchRate = 90;
    public Vector2 pitchRange = new Vector2(-89, 89);
    public bool InvertCamVerticle = true;
    protected Rigidbody rb;
    public bool IsGrounded = true;
    public float JumpSpeed;


    public List<MapInteractable> Interactables;
    public MapInteractable ObjectUsing;
   


    //bool for interact
    public bool InteractE = false;
    public bool lockMovement = false;


    //Properties
    public Containers playerInventory
    {
        get
        {
            if (!_PlayerInventory)
            {
                _PlayerInventory = gameObject.GetComponent<Containers>();
            }

            return _PlayerInventory;
        }
    }

    public void Start()
    {
        
        rb = gameObject.GetComponent<Rigidbody>();
        Initialize();

    }
    public virtual void Initialize()
    {

    }


    public void SetCamPitch(float value)
    {
        if(lockMovement)
        {
            return;
        }
        if (ObjectUsing)
        {
            return;
        }
        
        if (value == 0)
        {
            return;
        }

        if (InvertCamVerticle)
        {
            value *= -1;
        }

        float nextPitch = CameraControl.transform.rotation.eulerAngles.x;
        if (nextPitch > 180)
        {
            nextPitch -= 360;
        }

        float delta = (value * mouseSensitivity * pitchRate * Time.deltaTime);
        nextPitch = nextPitch + delta;

        // Restrain with in Riange
        if (nextPitch < pitchRange.x)
        {
            nextPitch = pitchRange.x;
        }

        if (nextPitch > pitchRange.y)
        {
            nextPitch = pitchRange.y;
        }

        Quaternion r = Quaternion.Euler(nextPitch, 0, 0);
        CameraControl.transform.localRotation = r;
    }

    public override void RotatePlayer(float value)
    {
        if (lockMovement)
        {
            return;
        }
        if (ObjectUsing)
        {
            return;
        }
        gameObject.transform.Rotate(Vector3.up * value * mouseSensitivity * rotationRate * Time.deltaTime);
    }

    public override void Move(float horizontal, float vertical)
    {
        if (lockMovement)
        {
            return;
        }
        if (ObjectUsing)
        {
            return;
        }
        if (!rb)
        {
            Debug.Log("waiting");
            return;
        }
        Vector3 direction = (gameObject.transform.forward * vertical) + (gameObject.transform.right * horizontal);
        direction = direction.normalized;

        rb.velocity = new Vector3(0,rb.velocity.y,0) + (direction * moveRate);
    }

    public override void Jump(bool s)
    {
        if (lockMovement)
        {
            return;
        }
        if (ObjectUsing)
        {
            return;
        }
        if (s)
        {
            Debug.Log("Jump in Player Pawn");
            if (IsGrounded)
            {
                Debug.Log("In Ground");
                rb.velocity = new Vector3(rb.velocity.x,JumpSpeed, rb.velocity.z);
                //rb.AddForce(Vector3.up * JumpSpeed);
                IsGrounded = false;
            }
        }
    }

    public override void Interact(bool e)
    {
        //how guards interact with prisoners will be different system
        if (e && !ObjectUsing &&  (Interactables.Count != 0))
        {
            Debug.Log($"pressed e: {e} ");
            if(Interactables[0].IsInUse())
            {
                Debug.Log("object is in use");
                //will need to add code here later to indicate the object is in use
            }
            else
            {
                if (Interactables[0].Use((PlayerController)control))
                {
                    ObjectUsing = Interactables[0];
                }    
            }
        }
    }

    public override void Close(bool escape)
    {
       
        if (escape && ObjectUsing)
        {
            EndInteract();
        }

      
    }
    
    public override void EndInteract()
    {
        if(ObjectUsing)
        {
            ObjectUsing.Done();      
        }
        ObjectUsing = null;
        lockMovement = false;
    }


    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "Ground")
        {
            IsGrounded = true;
        }
    }

    public override void Fire1()
    {
        if(IsServer)
        {
            SpawnProj();
        }
        else
        {
            InvokeServerRpc(SpawnProj);
        }
    }

    [ServerRPC(RequireOwnership = false)]
    public void SpawnProj()
    {
        NetworkSpawn(projPrefab, projSpawn.transform.position, projSpawn.transform.rotation);
    }

    
}
