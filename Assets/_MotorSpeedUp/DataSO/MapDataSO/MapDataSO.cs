using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapDataSO", menuName = "DataSO/MapDataSO", order = 1)]
public class MapDataSO : ScriptableObject
{
    [SerializeField] private List<GameObject> mapPrefabs;

    public GameObject GetMapPrefab(int level)
    {
        if (level < 0)
        {
            Debug.LogError("Level cannot be negative.");
            return null;
        }

        

        foreach (var mapPrefab in mapPrefabs)
        {
            var mapController = mapPrefab.GetComponent<MapController>();
            Debug.Log($"level: {mapController.MapID}");
            if (mapController == null)
            {
                Debug.LogError($"Map prefab {mapPrefab.name} does not have a MapController component.");
                
                continue;
            }
            if (mapController != null && mapController.MapID == level)
            {
                
                return mapPrefab;
            }
        }

        // TODO: level is out of range, maybe we can return the last map prefab or handle it differently
        Debug.LogError($"No map prefab found for level {level}.");
        return null;
    }
}
