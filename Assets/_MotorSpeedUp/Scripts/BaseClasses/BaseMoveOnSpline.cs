using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseMoveOnSpline
{
    private MapController mapController;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private Rigidbody rb;

    private float plusYVelocity = 0f;
    public void SetMapController(MapController mapController)
    {
        this.mapController = mapController;
    }
    public void GetClosestTrackPointBehind(ref TrackPoint trackPointBehind, Transform transform)
    {

        if (trackPointBehind.position == null)
        {
            trackPointBehind = mapController.TrackPoints[0];
        }


        Vector3 directionFromTrackPointToPlayer = transform.position - trackPointBehind.position;
        if (Vector3.Dot(trackPointBehind.rotation * Vector3.forward, directionFromTrackPointToPlayer) < 0)
        {
            if (trackPointBehind.index == 0)
            {
                Debug.LogWarning("Out of map");
                return;
            }
            for (int i = trackPointBehind.index - 1; i >= 0; i--)
            {
                TrackPoint trackPoint = mapController.TrackPoints[i];
                directionFromTrackPointToPlayer = transform.position - trackPoint.position;
                if (Vector3.Dot(trackPoint.rotation * Vector3.forward, directionFromTrackPointToPlayer) >= 0)
                {
                    trackPointBehind = trackPoint;
                    return;
                }
            }
        }
        else
        {
            for (int i = trackPointBehind.index + 1; i < mapController.TrackPoints.Count; i++)
            {
                TrackPoint trackPoint = mapController.TrackPoints[i];
                directionFromTrackPointToPlayer = transform.position - trackPoint.position;
                if (Vector3.Dot(trackPoint.rotation * Vector3.forward, directionFromTrackPointToPlayer) < 0)
                {

                    return;
                }
                trackPointBehind = trackPoint;
            }
        }
    }

    public void CalculateInterpolatedPosition(ref Quaternion interpolatedRotation, TrackPoint trackPointBehind, Transform transform)
    {
        TrackPoint trackPointAhead = mapController.TrackPoints[trackPointBehind.index + 1];
        Vector3 trackDirection = (trackPointAhead.position - trackPointBehind.position);
        float t = Vector3.Dot(transform.position - trackPointBehind.position, trackDirection) / trackDirection.sqrMagnitude;
        t = Mathf.Clamp01(t);

        interpolatedRotation = Quaternion.Slerp(trackPointBehind.rotation, trackPointAhead.rotation, t);

        // for debug draw
        Debug.DrawLine(trackPointBehind.position, trackPointAhead.position, Color.red);
        Vector3 interpolatedPosition = Vector3.Lerp(trackPointBehind.position, trackPointAhead.position, t);
        Debug.DrawLine(transform.position, interpolatedPosition, Color.green);
    }

    public void Move(Vector3 playerMoveDirection)
    {

        // we shouldn't accelerate gradually on each axis separately, so I discard this code as commented.
        //float targetZVelocity = playerMoveDirection.z * moveSpeed;
        //currentZVelocity = Mathf.MoveTowards(rb.velocity.z, targetZVelocity, acceleration * Time.fixedDeltaTime);
        //float targetXVelocity = playerMoveDirection.x * moveSpeed;
        //currentXVelocity = Mathf.MoveTowards(rb.velocity.x, targetXVelocity, acceleration * Time.fixedDeltaTime);
        //currentZVelocity = playerMoveDirection.z * moveSpeed;
        //currentXVelocity = playerMoveDirection.x * moveSpeed;

        Vector3 currentVelocity = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z);
        Vector3 targetVelocity = playerMoveDirection * moveSpeed;

        currentVelocity = Vector3.MoveTowards(currentVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
        rb.velocity = new Vector3(currentVelocity.x, currentVelocity.y, currentVelocity.z);

    }
}
