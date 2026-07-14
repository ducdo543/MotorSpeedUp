using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Time;

public class PlayerMovement : MonoBehaviour, IVehicleMovement
{
    private MapController mapController;
    private Rigidbody rb;

    [Header("Movement")]
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private BaseMoveOnSpline baseMoveOnSpline;
    private Quaternion targetRotation;
    private float newVerticalInput;
    private float newHorizontalInput;

    private InputHandleMovement inputHandleMovement;

    private Quaternion InterpolatedRotation => baseMoveOnSpline.InterpolatedRotation;

    [Header("Fields for check ground")]
    [SerializeField] private float castDistance = 0.2f;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private Vector3 halfExtents = new Vector3(0.1f, 0.1f, 0.5f);

    private void Start()
    {
        inputHandleMovement = GetComponent<InputHandleMovement>();
        mapController = GameObject.FindObjectOfType<MapController>();
        rb = GetComponent<Rigidbody>();

        baseMoveOnSpline.SetFields(mapController, rb, transform);
    }

    public void WorkingWithInput()
    {

        newVerticalInput = inputHandleMovement.VerticalInput;
        newHorizontalInput = inputHandleMovement.HorizontalInput;

        if (!IsGrounded())
        {
            newVerticalInput = 0;
        }
    }

    public void Move()
    {
        baseMoveOnSpline.Move(newVerticalInput, newHorizontalInput, IsGrounded());
    }

    public void GetRotation()
    {

        Vector3 playerMoveDirection = Vector3.zero;
        Vector3 velocityWithoutUp = Vector3.zero;

        if (!IsGrounded())
        {
            // we want to remove the up component of the velocity
            Vector3 velocityWithoutUpWorld = Vector3.ProjectOnPlane(rb.velocity, Vector3.up);
            // then project that onto the plane defined by the up axis of the spline
            velocityWithoutUp = Vector3.ProjectOnPlane(velocityWithoutUpWorld, InterpolatedRotation * Vector3.up);
        }
        else
        {
            // forward that is perpendicular to the up axis of the spline
            velocityWithoutUp = Vector3.ProjectOnPlane(rb.velocity, InterpolatedRotation * Vector3.up);
        }

        if (velocityWithoutUp.magnitude > 1f)
        {
            playerMoveDirection = velocityWithoutUp.normalized;
            targetRotation = Quaternion.LookRotation(playerMoveDirection);
        }
    }
    public void RotatePlayer()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

    }


    public void CalculateInterpolatedPosition()
    {
        baseMoveOnSpline.CalculateInterpolatedPosition();
    }

    public bool IsGrounded()
    {
        bool isGrounded = Physics.BoxCast(transform.position + transform.up * 1f, halfExtents, -transform.up, transform.rotation, castDistance, groundLayerMask);
        // + (0,1,0) to ensure the center of the box is above the ground, so it can detect the ground properly. Now we just need to change the castDistance 
        return isGrounded;
    }

    private void OnDrawGizmosSelected()
    {

        Vector3 start = transform.position + transform.up * 1f;
        Vector3 end = start - transform.up * castDistance;

        // draw the boxcast gizmo
        Gizmos.matrix = Matrix4x4.TRS(
            transform.position + new Vector3(0, 1f, 0),
            transform.rotation,
            Vector3.one);

        Gizmos.DrawWireCube(
            Vector3.zero,
            halfExtents * 2f);

        // draw castDistance line
        Gizmos.matrix = Matrix4x4.identity;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(start, end);
    }
}