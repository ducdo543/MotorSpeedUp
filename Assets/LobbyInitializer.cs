using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyInitializer : MonoBehaviour
{
    [SerializeField] private MotorBikerSpawner motorBikerSpawner;
    [SerializeField] private GameObject motorHolder;

    [SerializeField] private LobbyTireRotater lobbyTireRotater;
    void Start()
    {
        motorBikerSpawner.SpawnMotorBiker(motorHolder.transform);

        lobbyTireRotater.Initialize();
    }

    void Update()
    {
        lobbyTireRotater.UpdateTires();
    }
}
