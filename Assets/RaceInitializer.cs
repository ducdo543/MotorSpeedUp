using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MotorSpeedUp.Data;

public class RaceInitializer : MonoBehaviour
{
    [SerializeField] private CameraTargetController cameraTargetController;

    [Header("Prefabs")]
    [SerializeField] private GameObject motorGameplayPrefab;

    [Header("Other SerializeFields")]
    [SerializeField] private MotorBikerSpawner motorBikerSpawner;

    private GameObject motorGameplay;
    private GameObject map;

    private void Awake()
    {
        SpawnMap();

        // spawn motor and biker
        motorGameplay = Instantiate(motorGameplayPrefab, transform);
        motorBikerSpawner.SpawnMotorBiker(motorGameplay.transform);


        RegisterForCameraTarget();
        RegisterForMotorGameplay();
    }

    private void SpawnMap()
    {
        // note that GameManager need to be initialized first before getting the current level
        int currentLevel = GameManager.Instance.MotorSpeedUpData.Level;
        GameObject mapPrefab = DataReader.Instance.GetMapPrefab(currentLevel);

        if (mapPrefab == null)
        {
            Debug.LogError("Can't instantiate map prefab, Map prefab not found for level: " + currentLevel);
            return;
        }

        map = Instantiate(mapPrefab, transform);
    }    



    private void RegisterForCameraTarget()
    {
        if (motorGameplay != null)
        {
            cameraTargetController.RegisterVehicle(motorGameplay.transform);
        }
        else
        {
            Debug.LogError("motorGameplay is not instantiated, cannot register vehicle.");
        }
    }    

    private void RegisterForMotorGameplay()
    {
        MotorController motorController = motorGameplay.GetComponent<MotorController>();
        if (motorController != null)
        {
            motorController.Initialize(map);
        }
        else
        {
            Debug.LogError("motorGameplay prefab does not have a MotorController component.");
        }
    }    
}
