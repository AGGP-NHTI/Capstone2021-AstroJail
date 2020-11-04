using MLAPI;
using MLAPI.Messaging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller
{
    public GameObject PSpawn;

    private void Start()
    {
        SpawnPlayer();
    }

    private void Update()
    {
        if (IsLocalPlayer) return;
        if (!myPawn) return;

        float MouseX = Input.GetAxis("Mouse X");
        float MouseY = Input.GetAxis("Mouse Y");
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        bool fire1 = Input.GetButtonDown("Fire1");

        myPawn.GetComponent<PlayerPawn>().SetCamPitch(MouseY);
        myPawn.RotatePlayer(MouseX);
        myPawn.Move(h, v);

    }

    public void SpawnPlayer()
    {
        if (IsOwner)
        {
            InvokeServerRpc(Server_SpawnPlayer, OwnerClientId);
        }
    }

    [ServerRPC(RequireOwnership = false)]
    public void Server_SpawnPlayer(ulong whclient)
    {
        Debug.Log("Server_SpawnPlayer called");
        Vector3 position = new Vector3(0, 0, 0);
        GameObject Gobj = Instantiate(PSpawn, position, Quaternion.identity);
        Gobj.GetComponent<NetworkedObject>().SpawnWithOwnership(OwnerClientId);

        InvokeClientRpcOnClient(client_set, whclient, Gobj.GetComponent<NetworkedObject>().NetworkId);
    }

    [ClientRPC]
    public void client_set(ulong id)
    {
        myPawn = GetNetworkedObject(id).GetComponent<PlayerPawn>();
        myPawn.Possessed(this);
    }

        
    
        
}
