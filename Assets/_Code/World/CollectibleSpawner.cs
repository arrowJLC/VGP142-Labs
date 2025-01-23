using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class CollectibleSpawner : MonoBehaviour
{
    public List<Transform> spawnPoints;
    public List<GameObject> collectiblePrefabs;

    void Start()
    {
        SpawnCollectibles();
    }

    void SpawnCollectibles()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {

            GameObject collectible = collectiblePrefabs[Random.Range(0, collectiblePrefabs.Count)];

            Instantiate(collectible, spawnPoint.position, Quaternion.identity);
        }
    }
}