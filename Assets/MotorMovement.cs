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
    private float newVerticalInput;
    private float newHorizontalInput;
    [SerializeField] private WheelCollider[] wheelColliders;

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

    public void WorkingWithInput()
    {
        newVerticalInput = inputHandleMovement.VerticalInput;
        newHorizontalInput = Mathf.Clamp(inputHandleMovement.HorizontalInput, -maxHorizontalInput, maxHorizontalInput);

        if (!IsGrounded())
        {
            newVerticalInput = 0;
        }

        
    }

    public void Move()
    {
        
        baseMoveOnSpline.Move(interpolatedRotation, newVerticalInput, newHorizontalInput);
        for (int i = 0; i < wheelColliders.Length; i++)
        {
            wheelColliders[i].motorTorque = 0.01f;
            // this is to inform internal physics handle the movement case instead of the static case for wheelCollider
            // if not set motorTorque a little bit, even we add force by rb.AddForce, the motor can't move
        }
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
