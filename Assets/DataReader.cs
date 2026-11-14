using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MotorSpeedUp.Data;

// data reader is used to read data from ScriptableObjects
public class DataReader : MonoBehaviour
{
    public static DataReader Instance { get; private set; }
    [SerializeField] private MapDataSO mapDataSO;
    [SerializeField] private MotorDataBaseSO motorDataBaseSO;
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

    public Material GetFirstMaterialOfMap(int level)
    {
        GameObject mapPrefab = GetMapPrefab(level);
        if (mapPrefab == null)
        {
            Debug.LogError($"No map prefab found for level {level}.");
            return null;
        }
        Renderer renderer = mapPrefab.GetComponentInChildren<Renderer>();
        if (renderer == null || renderer.sharedMaterials.Length == 0)
        {
            Debug.LogError($"Map prefab {mapPrefab.name} does not have a Renderer with materials.");
            return null;
        }
        return renderer.sharedMaterials[0];
    }

    public void GetMotorBiker(MotorType motorType, BikerType bikerType, out GameObject motorPrefab, out GameObject bikerPrefab, out AnimationClip animationClip)
    {
        motorDataBaseSO.GetMotorBiker(motorType, bikerType, out motorPrefab, out bikerPrefab, out animationClip);
    }
}
