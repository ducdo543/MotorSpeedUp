using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleRevive : MonoBehaviour
{
    [SerializeField] private Transform map;

    [SerializeField] private MotorController motorController;

    private int startTrackPointIndex = 0;
    public void InitializeMap(Transform map)
    {
        this.map = map;
    }
    
    public GameObject GetRespawnPoint(TrackPoint FurthestTrackPoint)
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
        Transform respawnPoint = respawnPointHolder.GetChild(startTrackPointIndex);
        //if (respawnPoint.GetComponent<TrackPointFollower>().TrackPointIndexCorrespondingTo == FurthestTrackPoint.index)
        //{
        //    startTrackPointIndex = FurthestTrackPoint.index;
        //    respawnPoint = respawnPointHolder.GetChild(startTrackPointIndex);
        //}

        return respawnPoint.gameObject;
    }

}
