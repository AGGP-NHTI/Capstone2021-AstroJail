using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
public class SpawnPoints : NetworkBehaviour
{
    private static SpawnPoints _instance;
    public static SpawnPoints Instance { get { return _instance; } }

    public List<Vector3> guardSpawns, prisonerSpawns;

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
            Vector3 randomSpawn = prisonerSpawns[randomIndex];
            prisonerSpawns.RemoveAt(randomIndex);

            return randomSpawn;
        }
        else
        {
            int randomIndex = Random.Range(0, guardSpawns.Count - 1);
            Vector3 randomSpawn = guardSpawns[randomIndex];
            guardSpawns.RemoveAt(randomIndex);

            return randomSpawn;
        }
    }
}
