using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Time;

public class PlayerMovement : MonoBehaviour
{
    private InputHandleMovement inputHandleMovement;

    [Header("Movement")]
    [SerializeField] private float rotationSpeed = 10f;
    private Vector3 playerMoveDirection = new Vector3();
    [SerializeField] private BaseMoveOnSpline baseMoveOnSpline;

    [Header("Other serialized fields")]

    


    private TrackPoint trackPointBehind = new TrackPoint();
    private MapController mapController;
    private Quaternion interpolatedRotation;



    private void Start()
    {
        inputHandleMovement = GetComponent<InputHandleMovement>();
        mapController = GameObject.FindObjectOfType<MapController>();

        baseMoveOnSpline.SetMapController(mapController);
    }

    public void GetMoveDirection()
    {
        
        playerMoveDirection = interpolatedRotation * Vector3.forward * inputHandleMovement.VerticalInput + interpolatedRotation * Vector3.right * inputHandleMovement.HorizontalInput;
        
    }

    public void Move()
    {
        baseMoveOnSpline.Move(playerMoveDirection);
    }

    public void RotatePlayer()
    {
        if (playerMoveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(playerMoveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }


    public void CalculateInterpolatedPosition()
    {
        baseMoveOnSpline.GetClosestTrackPointBehind(ref trackPointBehind, transform);
        baseMoveOnSpline.CalculateInterpolatedPosition(ref interpolatedRotation, trackPointBehind, transform);
    }

}