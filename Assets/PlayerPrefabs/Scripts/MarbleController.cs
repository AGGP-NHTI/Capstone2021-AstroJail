

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarbleController : MonoBehaviour
{
    public GameObject CameraControl;
    public GameObject MarbleModel;


    public float MouseSensitivty = 15;
    public float MoveAccelleration = 1;
    public float MoveMaxSpeed = 15;
    public float RotationRate = 90;
    public float PitchRate = 90;
    public Vector2 PitchRange = new Vector2(-89, 89);
    public bool InvertCamVerticle = true;

    Rigidbody rb;

    Vector3 flipVel = Vector3.zero;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float MouseX = Input.GetAxis("Mouse X");
        float MouseY = Input.GetAxis("Mouse Y");
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        //SetCamPitch(MouseY);
        RotatePlayer(MouseX);
        Move(h, v);

    }

    void SetCamPitch(float value)
    {
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

        float delta = (value * MouseSensitivty * PitchRate * Time.deltaTime);
        nextPitch = nextPitch + delta;

        // Restrain with in Riange
        if (nextPitch < PitchRange.x)
        {
            nextPitch = PitchRange.x;
        }

        if (nextPitch > PitchRange.y)
        {
            nextPitch = PitchRange.y;
        }

        Quaternion r = Quaternion.Euler(nextPitch, 0, 0);
        CameraControl.transform.localRotation = r;
    }

    void RotatePlayer(float value)
    {
        gameObject.transform.Rotate(Vector3.up * value * MouseSensitivty * RotationRate * Time.deltaTime);
    }

    void Move(float horizontal, float vertical)
    {
        Vector3 Direction = (gameObject.transform.forward * vertical) + (gameObject.transform.right * horizontal);
        Direction = Direction.normalized;
        rb.velocity += Direction * MoveAccelleration;

        if (rb.velocity.magnitude > MoveMaxSpeed)
        {
            rb.velocity = rb.velocity.normalized * MoveMaxSpeed;
        }
        Debug.Log("S/D: " + rb.velocity.magnitude + " :: " + rb.velocity);

        // We need to flip the X and Z to get the correct Axis... 
        // Also the sign, to get the correct look of direction. 

        flipVel.x = -rb.velocity.z;
        flipVel.z = -rb.velocity.x;
        MarbleModel.transform.Rotate(flipVel * RotationRate * Time.deltaTime, Space.Self);
    }

    /* 
     * Marble Mat Credits: 
     * Hand Painted Stone Wall Texture
     * The Chayed
     * OGA-BY 3.0
     * https://opengameart.org/content/hand-painted-stone-wall-texture
    */
}
