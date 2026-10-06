using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MotorSpeedUp.Data;

public class RaceInitializer : MonoBehaviour
{
    [SerializeField] private CameraTargetController cameraTargetController;
    private GameObject motor;
    private GameObject map;

    private void Awake()
    {
        SpawnMap();
        SpawnMotorBiker();

        RegisterForCameraTarget();
        RegisterForMotor();
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

    private void SpawnMotorBiker()
    {
        GameObject motorPrefab = new GameObject();
        GameObject bikerPrefab = new GameObject();
        AnimationClip animationClip = new AnimationClip();


        MotorType motorType = GameManager.Instance.MotorSpeedUpData.MotorType;
        BikerType bikerType = GameManager.Instance.MotorSpeedUpData.BikerType;

        // get motor and biker prefabs and animation clip from the data reader
        DataReader.Instance.GetMotorBiker(motorType, bikerType, out motorPrefab, out bikerPrefab, out animationClip);


        motor = Instantiate(motorPrefab, transform);

        // adding biker into the same position of the biker socket of the motor
        Transform bikerSocket = motor.transform.Find("BikerSocket");
        GameObject biker = Instantiate(bikerPrefab, motor.transform);
        biker.transform.position = bikerSocket.position;

        // adding animation to the biker
        BikerSitAnimation bikerSitAnimation = biker.GetComponent<BikerSitAnimation>();
        if (bikerSitAnimation != null)
        {
            bikerSitAnimation.SetSitAnimation(animationClip);
        }
        else
        {
            Debug.LogError("Biker prefab does not have a BikerSitAnimation component.");
        }
    }

    private void RegisterForCameraTarget()
    {
        if (motor != null)
        {
            cameraTargetController.RegisterVehicle(motor.transform);
        }
        else
        {
            Debug.LogError("Motor is not instantiated, cannot register vehicle.");
        }
    }    

    private void RegisterForMotor()
    {
        MotorController motorController = motor.GetComponent<MotorController>();
        if (motorController != null)
        {
            motorController.Initialize(map);
        }
        else
        {
            Debug.LogError("Motor prefab does not have a MotorController component.");
        }
    }    
}
