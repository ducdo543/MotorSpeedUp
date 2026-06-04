using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerMovement playerMovement;



    //[Header("Other serialized fields")]
    private TrackPoint closestTrackPointBehind = new TrackPoint();

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();

    }
    // Update is called once per frame
    void Update()
    {
        playerMovement.GetNewDirection();
        playerMovement.RotatePlayer();
        GetClosestTrackPointBehind();
        Debug.Log("Closest track point behind: " + closestTrackPointBehind.index);
    }


    private void FixedUpdate()
    {
        playerMovement.MovePlayer();
    }

    private void GetClosestTrackPointBehind()
    {
        MapController mapController = GameObject.FindObjectOfType<MapController>();
        
        if (closestTrackPointBehind.position == null)
        {
            closestTrackPointBehind = mapController.TrackPoints[0];
        }
        

        Vector3 directionFromTrackPointToPlayer = transform.position - closestTrackPointBehind.position;
        if (Vector3.Dot(closestTrackPointBehind.rotation * Vector3.forward, directionFromTrackPointToPlayer) < 0)
        {
            if (closestTrackPointBehind.index == 0)
            {
                Debug.LogWarning("Out of map");
                return;
            }
            for (int i = closestTrackPointBehind.index - 1; i >= 0; i--)
            {
                TrackPoint trackPoint = mapController.TrackPoints[i];
                directionFromTrackPointToPlayer = transform.position - trackPoint.position;
                if (Vector3.Dot(trackPoint.rotation * Vector3.forward, directionFromTrackPointToPlayer) >= 0)
                {
                    closestTrackPointBehind = trackPoint;
                    return;
                }
            }
        }
        else
        {
            for (int i = closestTrackPointBehind.index + 1; i < mapController.TrackPoints.Count; i++)
            {
                TrackPoint trackPoint = mapController.TrackPoints[i];
                directionFromTrackPointToPlayer = transform.position - trackPoint.position;
                if (Vector3.Dot(trackPoint.rotation * Vector3.forward, directionFromTrackPointToPlayer) < 0)
                {
                    
                    return;
                }
                closestTrackPointBehind = trackPoint;
            }
        }

        //for (int i = startIndex; i >= 0; i--)
        //{
        //    TrackPoint trackPoint = mapController.TrackPoints[i];
        //    Vector3 directionFromTrackPointToPlayer = transform.position - trackPoint.position;
        //    if (Vector3.Dot(trackPoint.rotation * Vector3.forward, directionFromTrackPointToPlayer) < 0)
        //    {
        //        if (i == 0)
        //        {
        //            closestTrackPointBehind = trackPoint;
        //            Debug.LogWarning("Out of map");
        //            return;
        //        }
        //        TrackPoint trackPointBehind = mapController.TrackPoints[i - 1];
        //        closestTrackPointBehind = trackPointBehind;
        //    }
        //    else
        //    {
        //        if (closestTrackPointBehind.index != startIndex)
        //        {
        //            return;
        //        }
        //        break;
        //    }

        //}

        //for (int i = startIndex; i < mapController.TrackPoints.Count; i++)
        //{
        //    TrackPoint trackPoint = mapController.TrackPoints[i];

        //    Vector3 directionFromTrackPointToPlayer = transform.position - trackPoint.position;
        //    if (Vector3.Dot(trackPoint.rotation * Vector3.forward, directionFromTrackPointToPlayer) >= 0)
        //    {
        //        closestTrackPointBehind = trackPoint;
        //    }
        //    else
        //    {
        //        break;
        //    }

        //}
    }
}
