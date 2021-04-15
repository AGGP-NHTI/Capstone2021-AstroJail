using MLAPI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : NetworkBehaviour
{
    public bool Active = true;
    public bool CCImmune = false;
    
    public void NetworkSpawn(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        GameObject Gobj = Instantiate(prefab, position, rotation);
        Gobj.GetComponent<NetworkObject>().Spawn();
    }
}
