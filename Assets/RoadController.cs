using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadController : MonoBehaviour
{
    private GameObject mapPrefab;

    private void Awake()
    {
        // note that GameManager need to be initialized first before getting the current level
        int currentLevel = GameManager.Instance.CurrentLevel;
        mapPrefab = DataReader.Instance.GetMapPrefab(currentLevel);
        
        if (mapPrefab == null)
        {
            Debug.LogError("Can't instantiate map prefab, Map prefab not found for level: " + currentLevel);
            return;
        }

        GameObject map = Instantiate(mapPrefab, transform);
    }
}
