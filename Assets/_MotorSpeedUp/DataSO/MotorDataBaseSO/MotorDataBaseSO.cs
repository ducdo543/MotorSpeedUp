using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MotorSpeedUp.Data;

[CreateAssetMenu(fileName = "MotorDataBaseSO", menuName = "DataSO/MotorDataBaseSO", order = 2)]
public class MotorDataBaseSO : ScriptableObject
{
    [SerializeField] private List<MotorDataSO> motorDataSOs;
    [SerializeField] private BikerDataListSO bikerDataListSO;
    public void GetMotorBiker(MotorType motorType, BikerType bikerType, out GameObject motorPrefab, out GameObject bikerPrefab, out AnimationClip animationClip)
    {
        motorPrefab = null;
        bikerPrefab = null;
        animationClip = null;

        foreach (var bikerDataSO in bikerDataListSO.List)
        {
            if (bikerDataSO.BikerType == bikerType)
            {
                bikerPrefab = bikerDataSO.BikerPrefab;
                break;
            }
        }

        foreach (var motorDataSO in motorDataSOs)
        {
            if (motorDataSO.MotorType == motorType)
            {
                motorPrefab = motorDataSO.MotorPrefab;
                animationClip = motorDataSO.Clip;

                // get specific biker prefab and animation clip if available
                foreach (var motorBikerData in motorDataSO.SpecificMotorBikerDataList)
                {
                    if (motorBikerData.BikerDataSO.BikerType == bikerType)
                    {
                        bikerPrefab = motorBikerData.BikerDataSO.BikerPrefab;
                        if (motorBikerData.SpecificClip != null)
                        {
                            animationClip = motorBikerData.SpecificClip;
                            bikerPrefab = motorBikerData.BikerDataSO.BikerPrefab;
                        }
                        return;
                    }
                }
            }
        }
    }
}
