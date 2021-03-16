using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI.Serialization;
using System.IO;
using MLAPI.Serialization.Pooled;

[CreateAssetMenuAttribute(fileName = "newItem", menuName = "Astro Items")]


public class ItemDefinition : ScriptableObject
{
    public int itemId;
    public int instanceId=-1;
    public string ItemName = "Default Name";
    public GameObject ObjectModel;
    public Sprite imageArt;
    public string itemDescription = "insert Degen Humor Here";
    public bool hasEffect = false;
    public bool isContraband = false;
    public bool isCrafted = false;

    public List<itemEffects> Effects;

    public Containers startingLocation = null;
    
    public List<ItemDefinition> Recipe;
}

