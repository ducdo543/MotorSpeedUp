using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Time;

public class PlayerMovement : MonoBehaviour
{
    private InputHandlePlayer inputHandlePlayer;

    [Header("Movement")]
    [SerializeField] private float rotationSpeed = 10f;
    private Vector3 playerDirection = new Vector3();
    private float pastPlusYVelocity = 0f;
    [SerializeField] private BaseMoveOnSpline baseMoveOnSpline;

    [Header("Other serialized fields")]

    


    private TrackPoint trackPointBehind = new TrackPoint();
    private MapController mapController;
    private Quaternion interpolatedRotation;



    private void Start()
    {
        inputHandlePlayer = GetComponent<InputHandlePlayer>();
        mapController = GameObject.FindObjectOfType<MapController>();

        baseMoveOnSpline.SetMapController(mapController);
    }

    public void GetNewDirection()
    {
        inputHandlePlayer.HandleMovementInput();
        playerDirection = interpolatedRotation * Vector3.forward * inputHandlePlayer.VerticalInput + interpolatedRotation * Vector3.right * inputHandlePlayer.HorizontalInput;
        playerDirection.Normalize();
    }

    public void MovePlayer()
    {
        baseMoveOnSpline.MovePlayer(ref pastPlusYVelocity, playerDirection);
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