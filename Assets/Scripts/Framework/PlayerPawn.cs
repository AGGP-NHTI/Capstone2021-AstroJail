using MLAPI.Messaging;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerPawn : Pawn
{

    public GameObject flashLight;
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
    [Range(0.1f, 1f)]
    public float slerpRatio;


    public List<MapInteractable> Interactables;
    public MapInteractable ObjectUsing;
   

    public bool InteractE = false;
    public bool lockMovement = false;

    public GameObject reactorCountdown;
    public GameObject playerMenuPanel;
    public GameObject PlayerInventoryHUD;
    public GameObject optionsPanel;

    public bool inPlayerMenu = false;


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

    public virtual void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        PlayerInventoryHUD.SetActive(true);
        playerMenuPanel.SetActive(false);
        optionsPanel.SetActive(false);
        Initialize();
    }

    public override void Update()
    {
        base.Update();
        rotatePlayerModel();
    }

    public virtual void Initialize()
    {

    }

    public virtual void SetCamPitch(float value)
    {
       
        if (lockMovement)
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


    /// <summary>
    /// This function is quite possibly one of the most horrendous things I've written to date.
    /// I'm not proud of it, but I'm working with what I've got for animations
    /// </summary>
    public virtual void rotatePlayerModel()
    {
        if (!playerModel.activeSelf) return;

        Vector3 localVel = transform.InverseTransformDirection(rb.velocity);

        if (localVel.z > 1 && localVel.x > 1) //front right
        {
            playerModel.transform.localRotation = Quaternion.Slerp(playerModel.transform.localRotation, Quaternion.Euler(0, 45, 0), slerpRatio);
        }
        else if(localVel.z > 1 && localVel.x < -1) //front left
        {
            playerModel.transform.localRotation = Quaternion.Slerp(playerModel.transform.localRotation, Quaternion.Euler(0, -45, 0), slerpRatio);
        }
        else if(localVel.z < -1 && localVel.x > 1) //back right
        {
            playerModel.transform.localRotation = Quaternion.Slerp(playerModel.transform.localRotation, Quaternion.Euler(0, 135, 0), slerpRatio);
        }
        else if (localVel.z < -1 && localVel.x < -1) //back left
        {
            playerModel.transform.localRotation = Quaternion.Slerp(playerModel.transform.localRotation, Quaternion.Euler(0, -135, 0), slerpRatio);
        }
        else if (localVel.x > 1) //Right
        {
            playerModel.transform.localRotation = Quaternion.Slerp(playerModel.transform.localRotation, Quaternion.Euler(0, 90, 0), slerpRatio);
        }
        else if (localVel.x < -1) //Left
        {
            playerModel.transform.localRotation = Quaternion.Slerp(playerModel.transform.localRotation, Quaternion.Euler(0, -90, 0), slerpRatio);
        }
        else if (localVel.z < -1) //Backwards
        {
            playerModel.transform.localRotation = Quaternion.Slerp(playerModel.transform.localRotation, Quaternion.Euler(0, 180, 0), slerpRatio);
        }
        else //Forwards
        {
            playerModel.transform.localRotation = Quaternion.Slerp(playerModel.transform.localRotation, Quaternion.Euler(0, 0, 0), slerpRatio);
        }
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
                    Cursor.visible = true;
                }    
            }
        }
    }

    public override void FlashLight(bool f)
    {
        if(f)
        {
            if(flashLight.activeSelf)
            {
                flashLight.SetActive(false);
            }
            else
            {
                flashLight.SetActive(true);
            }
        }
    }

    public override void Close(bool escape)
    {
        if (escape && ObjectUsing)
        {
            EndInteract();
            return;
         }     
        if(escape && inPlayerMenu)
        {
            Cursor.visible = false;
            lockMovement = false;
            PlayerInventoryHUD.SetActive(true);
            playerMenuPanel.SetActive(false);
            optionsPanel.SetActive(false);
            inPlayerMenu = false;
            return;
        }
        else if(escape && !inPlayerMenu)
        {
            Cursor.visible = true;
            OpenPlayerMenu();
            return;
        }
       
    }
    
    public override void EndInteract()
    {
        Cursor.visible = false;
        if(ObjectUsing)
        {
            ObjectUsing.Done();      
        }
        ObjectUsing = null;
        lockMovement = false;
    }

    public override void OnPossessed()
    {
        if(control is PlayerController pc)
        {
            NamePlate.text = pc.playerName.Value;
        }
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
        
    }
    public override void OpenPlayerMenu()
    {
        lockMovement = true;
        PlayerInventoryHUD.SetActive(false);
        playerMenuPanel.SetActive(true);
        inPlayerMenu = true;

    }
    public override void OpenOptionsMenu()
    {
        PlayerInventoryHUD.SetActive(false);
        playerMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
        inPlayerMenu = true;
    }


}
