using MotorSpeedUp.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorBikerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject bikerRigGroupPrefab;

    public void SpawnMotorBiker(Transform parent)
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


        

        GameObject motor = Instantiate(motorPrefab, parent);

        GameObject bikerRigGroup = Instantiate(bikerRigGroupPrefab, motor.transform);

        GameObject biker = Instantiate(bikerPrefab, bikerRigGroup.transform);




        // register bones for the biker rig group
        BikerInformation bikerInformation = biker.GetComponent<BikerInformation>();
        BikerRigGroup bikerRigGroupScript = bikerRigGroup.GetComponent<BikerRigGroup>();
        RegisterForRigs(bikerInformation, bikerRigGroupScript);

        // after register the biker rig group and call rigBuider.Build(),
        // now we can set the sit animation for the biker
        // cause in some rigs, we use Maintain Target Offset, so animation should be set after the rig is built
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

}
