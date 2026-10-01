using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MotorSpeedUp.Data;

[CreateAssetMenu(fileName = "MotorDataBaseSO", menuName = "DataSO/MotorDataBaseSO", order = 2)]
public class MotorDataBaseSO : ScriptableObject
{
    [SerializeField] private List<MotorDataSO> motorDataSOs;

    public void GetMotorBiker(MotorType motorType, BikerType bikerType, out GameObject motorPrefab, out GameObject bikerPrefab, out AnimationClip animationClip)
    {
        motorPrefab = null;
        bikerPrefab = null;
        animationClip = null;

        foreach (var motorDataSO in motorDataSOs)
        {
            if (motorDataSO.MotorType == motorType)
            {
                motorPrefab = motorDataSO.MotorPrefab;
                foreach (var motorBikerData in motorDataSO.MotorBikerDataList)
                {
                    if (motorBikerData.BikerDataSO.BikerType == bikerType)
                    {
                        bikerPrefab = motorBikerData.BikerDataSO.BikerPrefab;
                        animationClip = motorBikerData.Clip;
                        return;
                    }
                }
            }
        }
    }
}
