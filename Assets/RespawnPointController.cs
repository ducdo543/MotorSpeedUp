using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPointController : MonoBehaviour
{
    public int trackPointIndexCorrespondingTo;
    private int _lastTrackPointIndexCorrespondingTo;
    [Header("check that track point value")]
    [ReadOnly]
    [SerializeField] private TrackPoint trackPoint;

   

    public void ChangeTransformBasedOnTrackPoint(MapController mapController = null)
    {
        if (mapController == null)
        {
            mapController = GetComponentInParent<MapController>();
        }
        
        if (mapController == null)
        {
            Debug.LogError("MapController not found.");
            return;
        }
        //debug object name that contains map controller
        Debug.Log($"MapController found: {mapController.gameObject.name}");
        List<TrackPoint> trackPoints = mapController.TrackPoints;

        if (trackPointIndexCorrespondingTo >= 0 && trackPointIndexCorrespondingTo < trackPoints.Count)
        {
            trackPoint = trackPoints[trackPointIndexCorrespondingTo];

            // change the position and rotation of the respawn point to match the track point
            transform.position = trackPoint.position;
            transform.rotation = trackPoint.rotation;
        }
        else
        {
            Debug.LogError("Track point index is out of range.");
            Debug.LogError($"trackPointIndexCorrespondingTo: {trackPointIndexCorrespondingTo}, trackPoints.Count: {trackPoints.Count}");
        }

    }

}
