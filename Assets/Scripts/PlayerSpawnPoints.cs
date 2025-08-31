using Unity.Netcode.Components;
using UnityEngine;
using System.Collections.Generic;

public class PlayerSpawnPoints : MonoBehaviour {
    public static PlayerSpawnPoints inst;
    [SerializeField]
    private List<GameObject> spawnPoints = new List<GameObject>();

    void Awake()
    {
        if (inst == null)
        {
            inst = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public GameObject GetRandomSpawnPoint()
    {
        if (spawnPoints.Count == 0)
            return null;
        return spawnPoints[Random.Range(0, spawnPoints.Count)];
    }
}