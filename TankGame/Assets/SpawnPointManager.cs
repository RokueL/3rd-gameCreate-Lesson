using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SpawnPointManager : MonoBehaviour
{
    [SerializeField] private NetworkStartPosition[] spawnPoints;

    private void Awake()
    {
        //spawnPoints = GetComponentsInChildren<NetworkStartPosition>();
        

        //spawnPoints = GameObject.FindObjectsOfType<NetworkStartPosition>();
    }

    public Transform GetNextStartPosition()
    {
        if(spawnPoints == null || spawnPoints.Length == 0)
        {
            throw new System.InvalidOperationException("spawn points is null or lenght 0");

        }
        int spawnID = Random.Range(0, spawnPoints.Length);
        return spawnPoints[spawnID].transform;
    }
}
