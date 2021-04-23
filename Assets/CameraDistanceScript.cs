using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDistanceScript : MonoBehaviour
{
    private float followDistance;
    public float cameraMaxFollow;
    public Transform cameraPivot;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        DetectCameraDistance();
        gameObject.transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, followDistance);
    }

    void DetectCameraDistance()
    {
        RaycastHit hit;
        Ray cameraRay = new Ray();
        cameraRay.origin = cameraPivot.position;
        cameraRay.direction = transform.forward * -1;


        if (Physics.Raycast(cameraRay, out hit, cameraMaxFollow))
        {
            followDistance = hit.distance * -1;
            Debug.Log(hit.collider.name);
        }
        else
        {
            followDistance = cameraMaxFollow * -1;
        }

        
    }
}
