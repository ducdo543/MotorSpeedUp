using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Time;

public class PlayerMovement : MonoBehaviour
{
    private InputHandlePlayer inputHandlePlayer;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private Rigidbody rb;
    private Vector3 playerDirection = new Vector3();
    private float pastPlusYVelocity = 0f;

    [Header("Other serialized fields")]
    [SerializeField] private Transform cameraTransform;


    private TrackPoint trackPointBehind = new TrackPoint();
    private MapController mapController;
    private Quaternion interpolatedRotation;


    private void Start()
    {
        inputHandlePlayer = GetComponent<InputHandlePlayer>();
        mapController = GameObject.FindObjectOfType<MapController>();
    }

    public void GetNewDirection()
    {
        inputHandlePlayer.HandleMovementInput();
        playerDirection = interpolatedRotation * Vector3.forward * inputHandlePlayer.VerticalInput + interpolatedRotation * Vector3.right * inputHandlePlayer.HorizontalInput;
        playerDirection.Normalize();
    }

    public void MovePlayer()
    {

        float currentYVelocity = rb.velocity.y - pastPlusYVelocity + (playerDirection.y * moveSpeed);
        rb.velocity = new Vector3(playerDirection.x * moveSpeed, currentYVelocity, playerDirection.z * moveSpeed);
        pastPlusYVelocity = (playerDirection.y * moveSpeed);
    }

    public void RotatePlayer()
    {
        if (playerDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(playerDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    public void GetClosestTrackPointBehind()
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

    public void CalculateInterpolatedPosition()
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

}