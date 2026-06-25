using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorMovement : MonoBehaviour
{
    private InputHandleMovement inputHandleMovement;

    [Header("Movement")]
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float maxLeanAngle = 45f;
    private Quaternion targetRotation;
    private float maxHorizontalInput = 0.7f;
    private Vector3 targetVelocity = new Vector3();

    [SerializeField] private BaseMoveOnSpline baseMoveOnSpline;

    [Header("Other serialized fields")]

    private TrackPoint trackPointBehind = new TrackPoint();
    private MapController mapController;
    private Rigidbody rb;
    private Quaternion interpolatedRotation;
    public Quaternion InterpolatedRotation => interpolatedRotation;

    [Header("Fields for check ground")]
    [SerializeField] private float castDistance = 0.2f;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private Vector3 halfExtents = new Vector3(0.1f, 0.1f, 0.5f);


    private void Start()
    {
        inputHandleMovement = GetComponent<InputHandleMovement>();
        mapController = GameObject.FindObjectOfType<MapController>();
        rb = GetComponent<Rigidbody>();

        baseMoveOnSpline.SetFields(mapController, rb);

    }

    private void Update()
    {
        if (!IsGrounded())
        {
            Debug.LogWarning("Player is not grounded!");
        }
    }

    public void GetMoveDirection()
    {
        Vector2 moveInput = new Vector2(inputHandleMovement.HorizontalInput / 2f, inputHandleMovement.VerticalInput).normalized;
        float newVerticalInput = moveInput.y;
        float newHorizontalInput = moveInput.x;

        if (!IsGrounded())
        {
            newVerticalInput = 0;
        }

        Vector3 verticalTargetVelocity = interpolatedRotation * Vector3.forward * newVerticalInput * baseMoveOnSpline.MoveSpeed;
        Vector3 horizontalTargetVelocity = interpolatedRotation * Vector3.right * Mathf.Clamp(
    newHorizontalInput,
    -maxHorizontalInput,
    maxHorizontalInput) * baseMoveOnSpline.MoveSpeed;
        
        targetVelocity = verticalTargetVelocity + horizontalTargetVelocity;
    }

    public void Move()
    {

        baseMoveOnSpline.Move(interpolatedRotation, targetVelocity);
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


    // check ground method
    private bool IsGrounded()
    {
        bool isGrounded = Physics.BoxCast(transform.position + new Vector3 (0, 1f, 0), halfExtents, Vector3.down, transform.rotation, castDistance, groundLayerMask);
        // + (0,1,0) to ensure the center of the box is above the ground, so it can detect the ground properly. Now we just need to change the castDistance 
        return isGrounded;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.matrix = Matrix4x4.TRS(
            transform.position + new Vector3(0, 1f, 0),
            transform.rotation,
            Vector3.one);

        Gizmos.DrawWireCube(
            Vector3.zero,
            halfExtents * 2f);
    }
}
