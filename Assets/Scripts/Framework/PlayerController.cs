using System;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Controller
{
    public int playerID;
    public bool myController = false;
    public NetworkVariable<string> playerName = new NetworkVariable<string>("DefaultName"); //Test this with someone else
    public NetworkVariable<int> playerEnum = new NetworkVariable<int>(0);
    public PlayerType selectedPlayerType; //Maybe make a NetworkVariable<int> to track this over the network
    public GameObject PSpawn;
    public bool usingGamePad = false;

    
    //Controller Inputs//
    Vector2 leftStick = Vector2.zero;
    Vector2 rightStick = Vector2.zero;
    float leftTrigger = 0.0f;
    float rightTrigger = 0.0f;

  
    bool buttonSpace = false;
    bool buttonInteract = false;
    bool buttonClose = false;
    bool buttonSearch = false;
    bool buttonLight = false;

    bool dpad_up = false;
    bool dpad_right = false;
    bool dpad_left = false;
    bool dpad_down = false;

    bool leftShoulder = false;
    bool rightShoulder = false;

    bool leftStickButton = false;
    bool rightStickButton = false;

    //Controller Inputs//

    int temp = 0;

    public override void Start()
    {
        base.Start();
        playerName.Settings.WritePermission = NetworkVariablePermission.OwnerOnly;
        playerEnum.Settings.WritePermission = NetworkVariablePermission.OwnerOnly;
        //SpawnPlayer();
        Debug.Log("My owner id is " + OwnerClientId);

        if(IsOwner)
        {
            myController = true;
        }
    }

    private void Update()
    {
        if (!IsLocalPlayer){
            return;
        }
        if (!myPawn){
            return;
        }

        GetInput();

        ((PlayerPawn)myPawn).SetCamPitch(rightStick.y);
        myPawn.RotatePlayer(rightStick.x);
        myPawn.Move(leftStick.x, leftStick.y);
        myPawn.Jump(buttonSpace);
        myPawn.Interact(buttonInteract);
        myPawn.Close(buttonClose);
        myPawn.Search(buttonSearch);
        myPawn.FlashLight(buttonLight);
    }

    private void GetInput()
    {
        leftStick = Vector2.zero;
        rightStick = Vector2.zero;

        if(usingGamePad)
        {
            bool gamePadConnected = GetInputGamePad();
            if (gamePadConnected)
            {
                return;
            }
        }

        GetInputKeyboardMouse();
    }

    private bool GetInputGamePad()
    {
        Gamepad gPad = Gamepad.current;

        if(gPad == null)
        {
            return false;
        }

        //TO-DO adding gamepad input here
        //
        //
        //
        //

        return true;
    }

    private void GetInputKeyboardMouse()
    {
        Keyboard KB = Keyboard.current;
        Mouse mouse = Mouse.current;


        rightStick.x = Input.GetAxis("Mouse X");
        rightStick.y = Input.GetAxis("Mouse Y");
        dpad_up = Input.GetKey(KeyCode.W);
        dpad_down = Input.GetKey(KeyCode.S);
        dpad_left = Input.GetKey(KeyCode.A);
        dpad_right = Input.GetKey(KeyCode.D);
        //buttonSouth = Input.GetKeyDown(KeyCode.E);
        buttonSpace = Input.GetKeyDown(KeyCode.Space);
        buttonInteract = Input.GetKeyDown(KeyCode.E);
        buttonClose = Input.GetKeyDown(KeyCode.Escape);
        buttonSearch = Input.GetKeyDown(KeyCode.Q);
        buttonLight = Input.GetKeyDown(KeyCode.F);

        KeyToAxis();
    }

    private void KeyToAxis()
    {
        if (dpad_up)
        {
            leftStick.y = 1;
        }
        if (dpad_down)
        {
            leftStick.y = -1;
        }
        if (dpad_right)
        {
            leftStick.x = 1;
        }
        if (dpad_left)
        {
            leftStick.x = -1;
        }
    }

    public void SpawnPlayer()
    {
        
        if(IsOwner)
        {
            SpawnPlayerServerRpc(OwnerClientId);
        }
        
        
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void SpawnPlayerServerRpc(ulong whclient)
    {
        Debug.Log("Server_SpawnPlayer called with owner id of " + whclient);
        Vector3 position = new Vector3(0, 0, 0);
        GameObject Gobj = Instantiate(PSpawn, position, Quaternion.identity);
        Gobj.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);

        //InvokeClientRpcOnClient(client_set, whclient, Gobj.GetComponent<NetworkObject>().NetworkId);
    }

    [ClientRpc]
    public void client_setClientRpc(ulong id)
    {
        Debug.Log("client_set with owner id of " + id);
        myPawn = GetNetworkObject(id).GetComponent<PlayerPawn>();
        myPawn.Possessed(this);
        myPawn.CameraControl.SetActive(true);
    }

    public void SpawnPlayerGameStart()
    {
        //Find a spawn point and set position to it
        //Set PSpawn to player prefab based on enum
        Vector3 position = SpawnPoints.Instance.randomSpawn(selectedPlayerType);
        GameObject Gobj = Instantiate(PSpawn, position, Quaternion.identity);
        Debug.Log("Spawning in: " + Gobj.name);
        Gobj.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);

        SetGameStartClientRpc(Gobj.GetComponent<NetworkObject>().NetworkObjectId, myClientParams);

    }

    [ClientRpc]
    public void SetGameStartClientRpc(ulong id, ClientRpcParams CRP = default)
    {
        myPawn = GetNetworkObject(id).GetComponent<Pawn>();
        myPawn.Possessed(this);
        myPawn.CameraControl.SetActive(true);
        myPawn.playerUI.SetActive(true);
        myPawn.NamePlate.gameObject.SetActive(false);
        
        if (myPawn.playerType == PlayerType.Guard)
        {
            foreach(SkinnedMeshRenderer smr in myPawn.playerModel.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                smr.enabled = false;
            }
           
        }
        
        myController = true;
        
        foreach(PlayerController pc in GameObject.FindObjectsOfType<PlayerController>())
        {
            foreach(PlayerPawn pp in GameObject.FindObjectsOfType<PlayerPawn>())
            {
                if(pc.OwnerClientId == pp.OwnerClientId)
                {
                    pp.NamePlate.text = pc.playerName.Value;
                }
            }
        }

        Cursor.visible = false;
    }
}
