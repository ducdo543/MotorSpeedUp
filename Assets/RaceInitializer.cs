using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MotorSpeedUp.Data;

public class RaceInitializer : MonoBehaviour
{
    [SerializeField] private CameraTargetController cameraTargetController;

    [Header("Prefabs")]
    [SerializeField] private GameObject motorGameplayPrefab;
    [SerializeField] private GameObject aiMotorGameplayPrefab;

    [Header("Fields For Registration")]
    [SerializeField] private UIRaceManager uiRaceManager;

    [Header("Other SerializeFields")]
    [SerializeField] private MotorBikerSpawner motorBikerSpawner;

    private GameObject motorGameplay;
    private GameObject aiMotorGameplay;
    private GameObject map;

    private void Awake()
    {
        SpawnMap();

        // spawn the main motor and biker
        motorGameplay = Instantiate(motorGameplayPrefab, transform);
        MotorType motorType = GameManager.Instance.DataManager.MotorSpeedUpData.MotorType;
        BikerType bikerType = GameManager.Instance.DataManager.MotorSpeedUpData.BikerType;
        motorBikerSpawner.SpawnMotorBiker(motorGameplay.transform, motorType, bikerType);

        // spawn the AI motor and biker, test with the same motor as the main motor first
        aiMotorGameplay = Instantiate(aiMotorGameplayPrefab, transform);
        MotorType aiMotorType = GameManager.Instance.DataManager.MotorSpeedUpData.MotorType;
        BikerType aiBikerType = GameManager.Instance.DataManager.MotorSpeedUpData.BikerType;
        motorBikerSpawner.SpawnMotorBiker(aiMotorGameplay.transform, aiMotorType, aiBikerType);

        RegisterForCameraTarget();

        // register for motor and ai motor
        MotorController motorController = motorGameplay.GetComponent<MotorController>();
        MotorController aiMotorController = aiMotorGameplay.GetComponent<MotorController>();
        RegisterForMotorGameplay(motorController);
        RegisterForMotorGameplay(aiMotorController);

        RegisterForMapGoalGate();
    }

    private void SpawnMap()
    {
        // note that GameManager need to be initialized first before getting the current level
        int currentLevel = GameManager.Instance.DataManager.MotorSpeedUpData.Level;
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

    private void RegisterForMotorGameplay(MotorController targetMotorController)
    {
        
        if (targetMotorController != null)
        {
            targetMotorController.Initialize(map);
        }
        else
        {
            Debug.LogError("targetMotorGameplay prefab does not have a MotorController component.");
        }
    }    

    private void RegisterForMapGoalGate()
    {
        GoalGateCollide goalGateCollide = map.GetComponentInChildren<GoalGateCollide>();
        if (goalGateCollide != null)
        {
            MotorController mainMotorController = motorGameplay.GetComponent<MotorController>();
            MotorController aiMotorController = aiMotorGameplay.GetComponent<MotorController>();
            goalGateCollide.Initialize(mainMotorController, aiMotorController, uiRaceManager);
        }
        else
        {
            Debug.LogError("Map prefab does not have a GoalGateCollide component.");
        }
    }
}
