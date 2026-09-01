using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class VehicleRevive : MonoBehaviour
{
    [SerializeField] private Transform map;

    [SerializeField] private Vector3 offSetPosition = new Vector3(0, 2, 0);
    //[SerializeField] private MotorController motorController;
    private IVehicleMovement vehicleMovement;

    public event Action<Quaternion> OnAfterRevive;

    private int childIndex = 0;
    public void InitializeMap(Transform map)
    {
        this.map = map;
    }

    // actually we can use binary search,
    // in the first place, i thought we always need to run this method in the Update method, but actually, just when the vehicle dies

    public void Initialize(IVehicleMovement vehicleMovement)
    {
        this.vehicleMovement = vehicleMovement;
    }
    private GameObject GetRespawnPoint(TrackPoint FurthestTrackPoint)
    {
        if (map == null)
        {
            Debug.LogError("Map is not assigned in VehicleRevive.");
            return null;
        }
        Transform respawnPointHolder = map.GetComponent<MapController>().RespawnPointHolder;
        if (respawnPointHolder == null)
        {
            Debug.LogError("RespawnPointHolder is not assigned in MapController.");
            return null;
        }

        while (true)
        {
            Transform respawnPoint = respawnPointHolder.GetChild(childIndex);
            //if (respawnPoint == null)
            //{
            //    if (childIndex == 0)
            //    {
            //        Debug.LogError($"don't have a respawn point");
            //        return null;
            //    }
            //    childIndex--;
            //    break; // don't have any respawn point left
            //}
            int trackPointIndexOfRespawn = respawnPoint.GetComponent<TrackPointFollower>().TrackPointIndexCorrespondingTo;
            if (trackPointIndexOfRespawn > FurthestTrackPoint.index)
            {
                childIndex--;
                break;
            }

            if (childIndex ==  respawnPointHolder.childCount - 1)
            {
                break;
            }
            childIndex++;
        }
        //if (respawnPoint.GetComponent<TrackPointFollower>().TrackPointIndexCorrespondingTo == FurthestTrackPoint.index)
        //{
        //    trackPointIndexOfRespawn = FurthestTrackPoint.index;
        //    respawnPoint = respawnPointHolder.GetChild(trackPointIndexOfRespawn);
        //}

        Transform surpassedRespawnPoint = respawnPointHolder.GetChild(childIndex); // get the respawn point that the player surpassed
        return surpassedRespawnPoint.gameObject;
    }

    public bool CheckDead()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            return true;
        }
        return false;
    }

    public void Revive()
    {
        GameObject currentRespawnPoint = GetRespawnPoint(vehicleMovement.FurthestTrackPoint);
        Vector3 position = currentRespawnPoint.transform.position;
        Quaternion quaternion = currentRespawnPoint.transform.rotation;

 
        transform.position = position + offSetPosition; // apply offset to the position
        transform.rotation = quaternion;


        // Reset the vehicle's movement state
        TrackPoint trackPointOfRespawn = currentRespawnPoint.GetComponent<TrackPointFollower>().TrackPoint;
        vehicleMovement.ResetVehicleFields(trackPointOfRespawn);
        Debug.Log($"{trackPointOfRespawn.index}");

        // Invoke the event
        OnAfterRevive?.Invoke(quaternion);
    }
}
