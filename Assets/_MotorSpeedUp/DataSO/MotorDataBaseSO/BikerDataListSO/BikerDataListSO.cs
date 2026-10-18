using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BikerDataListSO", menuName = "DataSO/BikerDataListSO", order = 4)]
public class BikerDataListSO : ScriptableObject
{
    [SerializeField] private List<BikerDataSO> bikerDataSOList;

    public List<BikerDataSO> List => bikerDataSOList;

}
