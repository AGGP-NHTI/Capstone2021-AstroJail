using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCamera : MonoBehaviour
{

    public float CameraTurnRate = 2f;
    public float Speed = 10f; 

    public GameObject theCamera;
    public Camera cam;
    public Transform t;

    public List<GameObject> cameraRunPoints;

    int nextPoint = 0;
    Vector3 MoveDirection = Vector3.zero;
    Vector3 previousMoveDirection = Vector3.zero;
    float ratePercent = 0f;
    float turnCounter = 0;
    float move;


    // Start is called before the first frame update
    void Start()
    {
        t = theCamera.GetComponent<Transform>();
        previousMoveDirection = theCamera.transform.forward;
        MoveDirection = (cameraRunPoints[nextPoint].transform.position - transform.position).normalized;
       
    }

    // Update is called once per frame
    void Update()
    {
        turnCounter += Time.deltaTime;
        move = Speed * Time.deltaTime;
        //moves camera to object 
        theCamera.transform.position = Vector3.MoveTowards(transform.position, cameraRunPoints[nextPoint].transform.position, move);

         ratePercent = turnCounter / CameraTurnRate;
        if(ratePercent>1)
        {
            ratePercent = 1;
        }

        theCamera.transform.forward = Vector3.Slerp(previousMoveDirection, MoveDirection, ratePercent);

   
        if (GetDistanceTo(cameraRunPoints[nextPoint]) < 3)
        {
            nextPoint++;
           
            if (nextPoint >= cameraRunPoints.Count)
            {
                nextPoint = 0;
            }
            previousMoveDirection = MoveDirection;
            MoveDirection = (cameraRunPoints[nextPoint].transform.position - transform.position).normalized;
            turnCounter = 0;
        }

    }
    public float GetDistanceTo(GameObject Other)
    {
        float distanceTo = (Other.transform.position - transform.position).magnitude;

        return distanceTo;
    }
}
