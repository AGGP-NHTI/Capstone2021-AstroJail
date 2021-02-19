using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverOutline : MonoBehaviour
{
    public GameObject sprite;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnMouseEnter()
    {
        sprite.GetComponent<SpriteRenderer>().enabled=true;
        
    }
    public void OnMouseExit()
    {
        sprite.GetComponent<SpriteRenderer>().enabled = false;
    }
}
