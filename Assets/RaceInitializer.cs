using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MotorSpeedUp.Data;

public class RaceInitializer : MonoBehaviour
{
    private GameObject mapPrefab;

    private void Awake()
    {
        SpawnMap();
        SpawnMotorBiker();
    }

    private void SpawnMap()
    {
        // note that GameManager need to be initialized first before getting the current level
        int currentLevel = GameManager.Instance.MotorSpeedUpData.Level;
        mapPrefab = DataReader.Instance.GetMapPrefab(currentLevel);

        if (mapPrefab == null)
        {
            Debug.LogError("Can't instantiate map prefab, Map prefab not found for level: " + currentLevel);
            return;
        }

        GameObject map = Instantiate(mapPrefab, transform);
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


        GameObject motor = Instantiate(motorPrefab, transform);

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
}
