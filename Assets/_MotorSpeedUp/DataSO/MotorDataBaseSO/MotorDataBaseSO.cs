using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MotorDataBaseSO", menuName = "DataSO/MotorDataBaseSO", order = 2)]
public class MotorDataBaseSO : ScriptableObject
{
    [SerializeField] private List<MotorDataSO> motorDataSOs;


}
