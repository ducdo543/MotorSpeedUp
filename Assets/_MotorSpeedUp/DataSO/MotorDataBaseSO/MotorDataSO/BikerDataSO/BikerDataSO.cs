using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MotorSpeedUp.Data;

[CreateAssetMenu(fileName = "BikerDataSO", menuName = "DataSO/BikerDataSO", order = 4)]
public class BikerDataSO : ScriptableObject
{
    [SerializeField] private BikerType bikerType;
    [SerializeField] private GameObject bikerPrefab;
}
