using System.Collections.Generic;
using UnityEngine;

public class PoweupSpawn : MonoBehaviour
{
    public List<Transform> spawnPoints; // Assign 5 Transform points in the Unity editor for spawn locations
    public List<GameObject> collectiblePrefabs; // Assign collectible prefabs in the editor

    void Start()
    {
        SpawnCollectibles();
    }

    void SpawnCollectibles()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            // Randomly select a collectible prefab from the list
            GameObject collectible = collectiblePrefabs[Random.Range(0, collectiblePrefabs.Count)];
            // Instantiate the collectible at the spawn point location
            Instantiate(collectible, spawnPoint.position, Quaternion.identity);

        }
    }
}
