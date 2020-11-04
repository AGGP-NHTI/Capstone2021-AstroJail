using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Spawning;
public class Server : MonoBehaviour
{
    public GameObject spawnPoint;

    private void Start()
    {
        SetUp();
    }
    private void SetUp()
    {
        NetworkingManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        NetworkingManager.Singleton.StartHost();
    }

    private void ApprovalCheck(byte[] connectionData, ulong clientId, MLAPI.NetworkingManager.ConnectionApprovedDelegate callback)
    {
        bool approve = true;
        bool createPlayerObject = true;

        ulong? prefabhash = null;

        callback(createPlayerObject, prefabhash, approve, spawnPoint.transform.position, spawnPoint.transform.rotation);
    }
}
