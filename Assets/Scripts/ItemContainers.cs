﻿using MLAPI.Prototyping;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI.Messaging;
using MLAPI;
using System.ComponentModel;

public class ItemContainers : NetworkedBehaviour
{
    public GameObject gamePannel;
    public GameObject Item;
    public bool IsPannelActive = false;
    public GameObject labelObject;
    Containers container;

    public void Start()
    {
        container = gameObject.GetComponent<Containers>();
        labelObject.SetActive(false);
    }


    public void OnTriggerEnter(Collider other)
    {
        labelObject.SetActive(true);
        PlayerController Pl = other.gameObject.GetComponent<PlayerController>();
        if (Pl && Input.GetKeyDown(KeyCode.E))
        {

            //Pl.canMove = false;
            gamePannel.SetActive(true);
            IsPannelActive = true;
        }
        //if(IsPannelActive == false)
        //{
        //    Pl.canMove = true;
        //}
    }
}

//This Script can look at whats in containers
//This script will also call from TakeItem and AddItem