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
    


    private TrackPoint trackPointBehind = new TrackPoint();
    private MapController mapController;
    private Quaternion interpolatedRotation;

    [Header("BaseClasses")]
    private BaseMoveOnSpline baseMoveOnSpline;

    private void Start()
    {
        inputHandlePlayer = GetComponent<InputHandlePlayer>();
        mapController = GameObject.FindObjectOfType<MapController>();

        baseMoveOnSpline = new BaseMoveOnSpline(mapController);
    }

    public void GetNewDirection()
    {
        inputHandlePlayer.HandleMovementInput();
        playerDirection = interpolatedRotation * Vector3.forward * inputHandlePlayer.VerticalInput + interpolatedRotation * Vector3.right * inputHandlePlayer.HorizontalInput;
        playerDirection.Normalize();
    }

    public void MovePlayer()
    {
        baseMoveOnSpline.MovePlayer(ref pastPlusYVelocity, playerDirection, moveSpeed, rb);
    }

    public void RotatePlayer()
    {
        if (playerDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(playerDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }


    public void CalculateInterpolatedPosition()
    {
        baseMoveOnSpline.GetClosestTrackPointBehind(ref trackPointBehind, transform);
        baseMoveOnSpline.CalculateInterpolatedPosition(ref interpolatedRotation, trackPointBehind, transform);
    }

}