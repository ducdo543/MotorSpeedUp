using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyInitializer : MonoBehaviour
{
    [SerializeField] private MotorBikerSpawner motorBikerSpawner;
    [SerializeField] private GameObject motorParentScale;
    void Start()
    {
        motorBikerSpawner.SpawnMotorBiker(motorParentScale.transform);
    }

    void Update()
    {
        
    }
}
