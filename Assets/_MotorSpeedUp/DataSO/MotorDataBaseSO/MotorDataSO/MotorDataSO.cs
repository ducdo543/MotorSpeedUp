using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MotorSpeedUp.Data;

[CreateAssetMenu(fileName = "MotorDataSO", menuName = "DataSO/MotorDataSO", order = 3)]
public class MotorDataSO : ScriptableObject
{
    [SerializeField] private MotorType motorType;
    [SerializeField] private GameObject motorPrefab;
    [SerializeField] private List<MotorBikerData> motorBikerDataList;
}

[System.Serializable]
public class MotorBikerData
{
    [SerializeField] private BikerDataSO bikerDataSO;
    [SerializeField] private AnimationClip clip;
}
