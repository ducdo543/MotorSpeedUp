using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorMovement : MonoBehaviour, IVehicleMovement
{

    [Header("Specific Fields")]
    private float maxHorizontalInput = 0.7f;
    private float newVerticalInput;
    private float newHorizontalInput;

    private InputHandleMovement inputHandleMovement;


    [Header("Base Fields")]
    [SerializeField] private MotorMovementBase motorMovementBase;

    public BaseMoveOnSpline BaseMoveOnSpline => motorMovementBase.BaseMoveOnSpline;
    public Quaternion InterpolatedRotation => BaseMoveOnSpline.InterpolatedRotation;
    public TrackPoint FurthestTrackPoint => BaseMoveOnSpline.FurthestTrackPoint;



    public void WorkingWithInput()
    {
        inputHandleMovement = GetComponent<InputHandleMovement>();

        newVerticalInput = inputHandleMovement.VerticalInput;
        newHorizontalInput = Mathf.Clamp(inputHandleMovement.HorizontalInput, -maxHorizontalInput, maxHorizontalInput) * 2f;

        if (!IsGrounded())
        {
            newVerticalInput = 0;
        }

        motorMovementBase.SetInput(newVerticalInput, newHorizontalInput);
    }

    public void SetNoInput()
    {
        newVerticalInput = 0;
        newHorizontalInput = 0;

        motorMovementBase.SetInput(newVerticalInput, newHorizontalInput);
    }

    public void Move()
    {

        BaseMoveOnSpline.Move(newVerticalInput, newHorizontalInput, IsGrounded());

        motorMovementBase.SetWheelTorque();
    }

    public bool IsGrounded()
    {
        return motorMovementBase.IsGrounded();
    }

    public void RotatePlayer()
    {

        motorMovementBase.RotatePlayer();

    }


    public void CalculateInterpolatedPosition()
    {
        motorMovementBase.CalculateInterpolatedPosition();
    }

    public void ResetVehicleFields(TrackPoint trackPoint)
    {
        motorMovementBase.ResetVehicleFields(trackPoint);
    }


    // this is called simultaneously with Start(), we haven't had rb,
    // so we have to GetComponent rb again
    public void Initialize(MapController mapController, Rigidbody rb, WheelCollider[] wheelColliders)
    {
        if (wheelColliders.Length == 0)
        {
            Debug.LogWarning("No WheelCollider found!", this);
        }

        motorMovementBase.Initialize(transform, mapController, rb, wheelColliders);


    }

    private void OnDrawGizmosSelected()
    {
        
        Vector3 start = transform.position + transform.up * 1f;
        Vector3 end = start - transform.up * motorMovementBase.CastDistance;

        // draw the boxcast gizmo
        Gizmos.matrix = Matrix4x4.TRS(
            transform.position + new Vector3(0, 1f, 0),
            transform.rotation,
            Vector3.one);

        Gizmos.DrawWireCube(
            Vector3.zero,
            motorMovementBase.HalfExtents * 2f);

        // draw castDistance line
        Gizmos.matrix = Matrix4x4.identity;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(start, end);
    }
}
