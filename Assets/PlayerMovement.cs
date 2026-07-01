using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Time;

public class PlayerMovement : MonoBehaviour
{
    private InputHandleMovement inputHandleMovement;

    [Header("Movement")]
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private BaseMoveOnSpline baseMoveOnSpline;
    private Quaternion targetRotation;
    private float newVerticalInput;
    private float newHorizontalInput;
    [Header("Other serialized fields")]

    private TrackPoint trackPointBehind = new TrackPoint();
    private MapController mapController;
    private Rigidbody rb;
    private Quaternion interpolatedRotation;



    private void Start()
    {
        inputHandleMovement = GetComponent<InputHandleMovement>();
        mapController = GameObject.FindObjectOfType<MapController>();
        rb = GetComponent<Rigidbody>();

        baseMoveOnSpline.SetFields(mapController, rb);
    }

    public void WorkingWithInput()
    {

        newVerticalInput = inputHandleMovement.VerticalInput;
        newHorizontalInput = inputHandleMovement.HorizontalInput;
    }

    public void Move()
    {
        baseMoveOnSpline.Move(interpolatedRotation, newVerticalInput, newHorizontalInput);
    }

    public void GetRotation()
    {
        Vector3 playerMoveDirection = interpolatedRotation * Vector3.forward * newVerticalInput + interpolatedRotation * Vector3.right * newHorizontalInput;
        if (playerMoveDirection != Vector3.zero)
        {
            targetRotation = Quaternion.LookRotation(playerMoveDirection);
        }
    }
    public void RotatePlayer()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

    }


    public void CalculateInterpolatedPosition()
    {
        baseMoveOnSpline.GetClosestTrackPointBehind(ref trackPointBehind, transform);
        baseMoveOnSpline.CalculateInterpolatedPosition(ref interpolatedRotation, trackPointBehind, transform);
    }

}