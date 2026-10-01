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

    public MotorType MotorType => motorType;
    public GameObject MotorPrefab => motorPrefab;
    public List<MotorBikerData> MotorBikerDataList => motorBikerDataList;
}

[System.Serializable]
public class MotorBikerData
{
    [SerializeField] private BikerDataSO bikerDataSO;
    [SerializeField] private AnimationClip clip;

    public BikerDataSO BikerDataSO => bikerDataSO;
    public AnimationClip Clip => clip;
}
