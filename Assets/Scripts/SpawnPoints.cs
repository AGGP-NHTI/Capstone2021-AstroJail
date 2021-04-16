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
            int randomIndex = Random.Range(0, prisonerSpawns.Count - 1);
            Transform randomSpawn = prisonerSpawns[randomIndex];

            removeSpawnServerRpc(randomIndex, playerEnum);
            return randomSpawn.position;
        }
        else
        {
            int randomIndex = Random.Range(0, guardSpawns.Count - 1);
            Transform randomSpawn = guardSpawns[randomIndex];

            removeSpawnServerRpc(randomIndex, playerEnum);
            return randomSpawn.position;
        }
    }


    [ServerRpc(RequireOwnership = false)]
    public void removeSpawnServerRpc(int index, PlayerType playerEnum)
    {
        if(playerEnum == PlayerType.Prisoner)
        {
            prisonerSpawns.RemoveAt(index);
        }
        else
        {
            guardSpawns.RemoveAt(index);
        }

        removeSpawnClientRpc(index, playerEnum);
    }

    [ClientRpc]
    public void removeSpawnClientRpc(int index, PlayerType playerEnum)
    {
        if (IsServer) return;
        if (playerEnum == PlayerType.Prisoner)
        {
            prisonerSpawns.RemoveAt(index);
        }
        else
        {
            guardSpawns.RemoveAt(index);
        }
    }
}
