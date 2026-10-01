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

    public void GetMotorBiker(MotorType motorType, BikerType bikerType, out GameObject motorPrefab, out GameObject bikerPrefab, out AnimationClip animationClip)
    {
        motorDataBaseSO.GetMotorBiker(motorType, bikerType, out motorPrefab, out bikerPrefab, out animationClip);
    }
}
