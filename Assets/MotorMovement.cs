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
    private float maxHorizontalInput = 0.7f;

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
        
        playerMoveDirection = interpolatedRotation * Vector3.forward * inputHandleMovement.VerticalInput 
    + interpolatedRotation * Vector3.right * Mathf.Clamp(
    inputHandleMovement.HorizontalInput,
    -maxHorizontalInput,
    maxHorizontalInput)/1.5f;
        //playerMoveDirection = new Vector3(playerMoveDirection.x, 0, playerMoveDirection.z);
    }

    public void Move()
    {
        baseMoveOnSpline.Move(playerMoveDirection);
    }

    public void GetRotation()
    {
        Quaternion uprightRotation = interpolatedRotation;
        //Debug.Log($"interpolatedRotation: {uprightRotation * Vector3.forward}");
        float leanAngle = -(maxLeanAngle * inputHandleMovement.HorizontalInput);
        Quaternion leanRotation = Quaternion.AngleAxis(leanAngle, uprightRotation * Vector3.forward);
        targetRotation = leanRotation * uprightRotation;

        //Debug.Log($"targetRotation: {targetRotation * Vector3.forward}");
    //    Debug.Log(
    //Vector3.Angle(
    //    uprightRotation * Vector3.forward,
    //    targetRotation * Vector3.forward));
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
