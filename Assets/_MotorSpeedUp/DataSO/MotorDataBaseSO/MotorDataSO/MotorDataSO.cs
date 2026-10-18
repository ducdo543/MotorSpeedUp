using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MotorSpeedUp.Data;

[CreateAssetMenu(fileName = "MotorDataSO", menuName = "DataSO/MotorDataSO", order = 3)]
public class MotorDataSO : ScriptableObject
{
    [SerializeField] private MotorType motorType;
    [SerializeField] private GameObject motorPrefab;
    [SerializeField] private AnimationClip clip;

    [SerializeField] private List<SpecificMotorBikerData> specificMotorBikerDataList;

    public MotorType MotorType => motorType;
    public GameObject MotorPrefab => motorPrefab;
    public AnimationClip Clip => clip;
    public List<SpecificMotorBikerData> SpecificMotorBikerDataList => specificMotorBikerDataList;
}

[System.Serializable]
public class SpecificMotorBikerData
{
    [SerializeField] private BikerDataSO bikerDataSO;
    [SerializeField] private AnimationClip specificClip;

    public BikerDataSO BikerDataSO => bikerDataSO;
    public AnimationClip SpecificClip => specificClip;
}
