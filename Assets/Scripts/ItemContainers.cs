﻿using MLAPI.Prototyping;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI.Messaging;
using MLAPI;
using System.ComponentModel;
using System.Reflection.Emit;
using TMPro;

public class ItemContainers : NetworkedBehaviour
{
    public GameObject gamePanel;
    public GameObject Item;
    public bool IsPanelActive = false;
    public GameObject labelObject;
    Containers container;
    public bool InUse = false;
    bool NextToContainer=false;
 

    public void Start()
    {
        container = gameObject.GetComponent<Containers>();
        labelObject.SetActive(false);
    }

    public void Update()
    {
        if(NextToContainer == true && Input.GetKeyDown(KeyCode.E) && !InUse)
        {
            OpenContainer();
        }
        if(InUse == true && Input.GetKeyDown(KeyCode.Escape))
        {
            InUse = false;
            gamePanel.SetActive(false);
            labelObject.GetComponent<TextMeshPro>().text = "Press E to Interact";
            if(IsServer)
            {
                InvokeClientRpcOnEveryone(Client_StopUse);
            }
            else
            {
                InvokeServerRpc(Server_StopUse);
            }
                
                   
        }
    }


    public void OnTriggerEnter(Collider other)
    {
        Controller P1C = other.gameObject.GetComponent<PlayerPawn>().control;
        PlayerController P1 = (PlayerController)P1C;
        labelObject.SetActive(true);
        if (P1)
        {
            NextToContainer = true;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        labelObject.SetActive(false);
    }

    public void OpenContainer()
    {
        
        InUse = true;
        gamePanel.SetActive(true);
        IsPanelActive = true;
        labelObject.GetComponent<TextMeshPro>().text = "In Use";

        if(IsServer)
        {
            InvokeClientRpcOnEveryone(Client_InUse);
        }
        else
        {
            InvokeServerRpc(Server_InUse);
        }
    }
    [ClientRPC]
    public void Client_InUse()
    {
        InUse = true;
        labelObject.GetComponent<TextMeshPro>().text = "In Use";
    }
    public void Client_StopUse()
    {
        InUse = false;
        labelObject.GetComponent<TextMeshPro>().text = "Press E to Interact";
    }
    [ServerRPC(RequireOwnership = false)]
    public void Server_InUse()
    {
        InUse = true;
        labelObject.GetComponent<TextMeshPro>().text = "In Use";
    }
    public void Server_StopUse()
    {
        InUse = false;
        labelObject.GetComponent<TextMeshPro>().text = "Press E to Interact";
    }
}

//This Script can look at whats in containers
//This script will also call from TakeItem and AddItem
//enum that checks for guard or prisoner