using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
public class Lightswitch : NetworkBehaviour
{
    public bool turnLightsOff;
    public float lightIntensity;
    public float lightRange;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<PrisonerPawn>())
        {
            TurnLightsOffServerRpc(turnLightsOff);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void TurnLightsOffServerRpc(bool lightsOff)
    {
        List<Light> lights = new List<Light>(GameObject.FindObjectsOfType<Light>());

        if(lightsOff)
        {
            foreach(Light l in lights)
            {
                l.range = 0;
                l.intensity = 0;
            }
        }
        else
        {
            foreach (Light l in lights)
            {
                l.range = lightRange;
                l.intensity = lightIntensity;
            }
        }
        TurnLightsOffClientRpc(lightsOff);
    }

    [ClientRpc]
    public void TurnLightsOffClientRpc(bool lightsOff)
    {
        if (IsServer) { return; }

        List<Light> lights = new List<Light>(GameObject.FindObjectsOfType<Light>());

        if (lightsOff)
        {
            foreach (Light l in lights)
            {
                l.range = 0;
                l.intensity = 0;
            }
        }
        else
        {
            foreach (Light l in lights)
            {
                l.range = 25;
                l.intensity = 2.5f;
            }
        }
    }
}
