using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MotorMovementBase
{
    [SerializeField] private MapController mapController;
    private Rigidbody rb;
    private Transform transform; // transform of the motor, we will set it in Initialize method

    [Header("Movement")]
    [SerializeField] private float rotationSpeed = 4f;
    [SerializeField] private float leanAngleMultiplier = 22f;
    private Quaternion targetRotation;
    private float newVerticalInput;
    private float newHorizontalInput;

    [SerializeField] private WheelCollider[] wheelColliders;
    [SerializeField] private BaseMoveOnSpline baseMoveOnSpline;
    public BaseMoveOnSpline BaseMoveOnSpline => baseMoveOnSpline;

    [Header("Fields for check ground")]
    [SerializeField] private float castDistance = 0.2f;
    //[SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private Vector3 halfExtents = new Vector3(0.1f, 0.1f, 0.5f);
    private readonly RaycastHit[] hits = new RaycastHit[10];

    // Properties to draw gizmos in the editor
    public float CastDistance => castDistance;
    public Vector3 HalfExtents => halfExtents;

    private void GetRotation()
    {
        Quaternion uprightRotation = baseMoveOnSpline.InterpolatedRotation;
        //Debug.Log($"interpolatedRotation: {uprightRotation * Vector3.forward}");
        float leanAngle = -(leanAngleMultiplier * newHorizontalInput);
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
        GetRotation();
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

    }

    public void CalculateInterpolatedPosition()
    {
        baseMoveOnSpline.CalculateInterpolatedPosition(IsGrounded());
    }

    // check ground method
    public bool IsGrounded()
    {
        int hitCount = Physics.BoxCastNonAlloc(
            transform.position + transform.up * 1f,
            halfExtents,
            -transform.up,
            hits,
            transform.rotation,
            castDistance
        );

        // Ignore self-collisions
        for (int i = 0; i < hitCount; i++)
        {
            Collider hitCollider = hits[i].collider;

            if (hitCollider.transform.IsChildOf(transform))
            {
                continue;
            }

            return true;
        }

        return false;

        //bool isGrounded = Physics.BoxCast(transform.position + transform.up * 1f, halfExtents, - transform.up, transform.rotation, castDistance);
        //// + (0,1,0) to ensure the center of the box is above the ground, so it can detect the ground properly. Now we just need to change the castDistance 
        //return isGrounded;
    }

    public void ResetVehicleFields(TrackPoint trackPoint)
    {
        baseMoveOnSpline.ResetVehicleFields(trackPoint);
    }

    // this is called simultaneously with Start(), we haven't had rb,
    // so we have to GetComponent rb again
    public void Initialize(Transform transform, MapController mapController, Rigidbody rb, WheelCollider[] wheelColliders)
    {
        this.mapController = mapController;
        this.rb = rb;
        this.transform = transform;

        this.wheelColliders = wheelColliders;
        if (wheelColliders.Length == 0)
        {
            Debug.LogWarning("No WheelCollider found!");
        }

        baseMoveOnSpline.SetFields(mapController, rb, transform);
    }

    public void SetInput(float verticalInput, float horizontalInput)
    {
        newVerticalInput = verticalInput;
        newHorizontalInput = horizontalInput;
    }

    public void SetWheelTorque()
    {
        if (newVerticalInput == 0 && newHorizontalInput == 0)
        {
            for (int i = 0; i < wheelColliders.Length; i++)
            {
                wheelColliders[i].motorTorque = 0f;
            }
        }
        else
        {
            for (int i = 0; i < wheelColliders.Length; i++)
            {
                wheelColliders[i].motorTorque = 0.01f;
                // this is to inform internal physics handle the movement case instead of the static case for wheelCollider
                // if not set motorTorque a little bit, even we add force by rb.AddForce, the motor can't move
            }
        }
    }
}
