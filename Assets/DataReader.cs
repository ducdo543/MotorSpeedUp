using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// data reader is used to read data from ScriptableObjects
public class DataReader : MonoBehaviour
{
    public static DataReader Instance { get; private set; }
    [SerializeField] private MapDataSO mapDataSO;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameObject GetMapPrefab(int level)
    {
        return mapDataSO.GetMapPrefab(level);
    }
}
