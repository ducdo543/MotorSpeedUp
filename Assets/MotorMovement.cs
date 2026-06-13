using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorMovement : MonoBehaviour
{
    private InputHandleMovement inputHandleMovement;

    [Header("Movement")]
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float maxLeanAngle = 45f;
    private Vector3 playerMoveDirection = new Vector3();
    private Quaternion targetRotation;

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
        playerMoveDirection.Normalize();
    }

    public void Move()
    {
        baseMoveOnSpline.Move(playerMoveDirection);
    }

    public void GetRotation()
    {
        Quaternion uprightRotation = interpolatedRotation;

        float leanAngle = -(maxLeanAngle * inputHandleMovement.HorizontalInput);
        Quaternion leanRotation = Quaternion.AngleAxis(leanAngle, uprightRotation * Vector3.forward);
        targetRotation = uprightRotation * leanRotation;
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
