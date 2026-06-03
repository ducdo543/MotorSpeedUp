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
        int startIndex = 0;
        if (closestTrackPointBehind.position == null)
        {
            startIndex = 0;
        }
        else
        {
            startIndex = closestTrackPointBehind.index;
        }
        for (int i = startIndex; i < mapController.TrackPoints.Count; i++)
        {
            TrackPoint trackPoint = mapController.TrackPoints[i];

            Vector3 directionFromTrackPointToPlayer = transform.position - trackPoint.position;
            if (Vector3.Dot(trackPoint.rotation * Vector3.forward, directionFromTrackPointToPlayer) > 0)
            {
                closestTrackPointBehind = trackPoint;
            }
            else
            {
                break;
            }
            //Vector3 directionToTrackPoint = trackPoint.position - transform.position;
            //if (Vector3.Dot(transform.forward, directionToTrackPoint) < 0)
            //{
            //    closestTrackPointBehind = trackPoint;
            //}
            //else
            //{
            //    break;
            //}
        }
    }
}
