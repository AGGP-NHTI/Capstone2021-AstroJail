using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
public class ReactorCleanup : NetworkBehaviour
{
    private float timer = 2f;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (IsServer)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                NuclearReactorEndClientRpc();
            }
        }
    }

    [ClientRpc]
    public void NuclearReactorEndClientRpc()
    {
        foreach (PlayerController pc in GameObject.FindObjectsOfType<PlayerController>())
        {
            if (pc.myController)
            {
                pc.myPawn.CameraControl.SetActive(false);
                pc.myPawn.playerUI.SetActive(false);
                Cursor.visible = true;
            }
        }
    }
}
