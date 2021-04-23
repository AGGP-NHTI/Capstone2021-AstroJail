using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowScript : MonoBehaviour
{
    public GameObject player;
    public float moveSpeed;
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        cameraFollow();  
    }

    void cameraFollow()
    {
        Vector3 offset = new Vector3(0, 0.5f, 0);
        transform.position = Vector3.Lerp(transform.position, player.transform.position + offset, Time.deltaTime * moveSpeed);
    }
}
