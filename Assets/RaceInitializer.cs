using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MotorSpeedUp.Data;

public class RaceInitializer : MonoBehaviour
{
    [SerializeField] private CameraTargetController cameraTargetController;

    [Header("Prefabs")]
    [SerializeField] private GameObject motorGameplayPrefab;
    [SerializeField] private GameObject bikerRigGroupPrefab;

    private GameObject motorGameplay;
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

        // when using new GameObject(), it will create an empty game object, we need to destroy it later
        GameObject emptyMotor = motorPrefab;
        GameObject emptyBiker = bikerPrefab;


        MotorType motorType = GameManager.Instance.MotorSpeedUpData.MotorType;
        BikerType bikerType = GameManager.Instance.MotorSpeedUpData.BikerType;

        // get motor and biker prefabs and animation clip from the data reader
        DataReader.Instance.GetMotorBiker(motorType, bikerType, out motorPrefab, out bikerPrefab, out animationClip);

        // destroy the empty motor and biker game objects
        Destroy(emptyMotor);
        Destroy(emptyBiker);


        motorGameplay = Instantiate(motorGameplayPrefab, transform);

        GameObject motor = Instantiate(motorPrefab, motorGameplay.transform);

        GameObject bikerRigGroup = Instantiate(bikerRigGroupPrefab, motor.transform);

        GameObject biker = Instantiate(bikerPrefab, bikerRigGroup.transform);

        // biker needs to be the same position of the biker socket
        Transform bikerSocket = motor.GetComponent<MotorInformation>().BikerSocket;
        biker.transform.position = bikerSocket.position;

        // adding animation to the biker
        BikerSitAnimation bikerSitAnimation = bikerRigGroup.GetComponent<BikerSitAnimation>();
        if (bikerSitAnimation != null)
        {
            bikerSitAnimation.SetSitAnimation(animationClip);
        }
        else
        {
            Debug.LogError("Biker rig group prefab does not have a BikerSitAnimation component.");
        }


        // register bones for the biker rig group
        BikerInformation bikerInformation = biker.GetComponent<BikerInformation>();
        BikerRigGroup bikerRigGroupScript = bikerRigGroup.GetComponent<BikerRigGroup>();
        RegisterForRigs(bikerInformation, bikerRigGroupScript);


    }

    private void RegisterForRigs(BikerInformation bikerInformation, BikerRigGroup bikerRigGroupScript)
    {
        if (bikerInformation != null && bikerRigGroupScript != null)
        {
            bikerRigGroupScript.SetRigsData(bikerInformation);
        }
        else
        {
            Debug.LogError("BikerInformation or BikerRigGroup is not instantiated, cannot register biker rig group.");
        }
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

    private void RegisterForMotor()
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
