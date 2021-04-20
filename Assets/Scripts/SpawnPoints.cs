using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
public class SpawnPoints : NetworkBehaviour
{
    private static SpawnPoints _instance;
    public static SpawnPoints Instance { get { return _instance; } }
    public List<Transform> guardSpawns, prisonerSpawns;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    
    public Vector3 randomSpawn(PlayerType playerEnum)
    {

        if (playerEnum == PlayerType.Prisoner)
        {
            if (prisonerSpawns.Count == 0) return Vector3.zero;
            int randomIndex = Random.Range(0, prisonerSpawns.Count - 1);
            Transform randomSpawn = prisonerSpawns[randomIndex];

            removePrisonerSpawnServerRpc(randomIndex);
            return randomSpawn.position;
        }
        else
        {
            if (guardSpawns.Count == 0) return Vector3.zero;
            int randomIndex = Random.Range(0, guardSpawns.Count - 1);
            Transform randomSpawn = guardSpawns[randomIndex];

            removeGuardSpawnServerRpc(randomIndex);
            return randomSpawn.position;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void removePrisonerSpawnServerRpc(int index)
    {
        prisonerSpawns.RemoveAt(index);
        Debug.Log("Calling prisoner clientRPC");
    }

    [ServerRpc(RequireOwnership = false)]
    public void removeGuardSpawnServerRpc(int index)
    {
        guardSpawns.RemoveAt(index);
        Debug.Log("Calling clientRPC");
    }


}
