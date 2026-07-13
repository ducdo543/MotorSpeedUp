using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseMoveOnSpline
{
    private MapController mapController;
    [SerializeField] private float moveSpeed = 5f;
    public float MoveSpeed => moveSpeed;

    [Tooltip("When enabled, acceleration and deceleration are ignored. The object instantly reaches the target speed.")]
    [SerializeField] private bool acchieveTargetSpeedInstantly = false;

    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float decelerationForward = 10f;
    [SerializeField] private float decelerationRight = 10f;
    private float decelerationOnSky = 10f; 
    [SerializeField] private Rigidbody rb;
    private Quaternion previousRotation;
    public Quaternion PreviousRotation => previousRotation;
    public void SetFields(MapController mapController, Rigidbody rb)
    {
        this.mapController = mapController;
        this.rb = rb;

        // decelerationOnSky should be really small compare to rb.mass / Time.fixedDeltaTime, otherwise the player will move upwards when not grounded (cause forward is canceled out, but velocity.y is still not 0, so the player will move upwards)
        decelerationOnSky = (rb.mass / Time.fixedDeltaTime) / 50f;
    }
    public void GetClosestTrackPointBehind(ref TrackPoint trackPointBehind, Transform transform)
    {

        if (trackPointBehind.position == null)
        {
            trackPointBehind = mapController.TrackPoints[0];
        }


        Vector3 directionFromTrackPointToPlayer = transform.position - trackPointBehind.position;
        if (Vector3.Dot(trackPointBehind.rotation * Vector3.forward, directionFromTrackPointToPlayer) < 0)
        {
            if (trackPointBehind.index == 0)
            {
                Debug.LogWarning("Out of map");
                return;
            }
            for (int i = trackPointBehind.index - 1; i >= 0; i--)
            {
                TrackPoint trackPoint = mapController.TrackPoints[i];
                directionFromTrackPointToPlayer = transform.position - trackPoint.position;
                if (Vector3.Dot(trackPoint.rotation * Vector3.forward, directionFromTrackPointToPlayer) >= 0)
                {
                    trackPointBehind = trackPoint;
                    return;
                }
            }
        }
        else
        {
            for (int i = trackPointBehind.index + 1; i < mapController.TrackPoints.Count; i++)
            {
                TrackPoint trackPoint = mapController.TrackPoints[i];
                directionFromTrackPointToPlayer = transform.position - trackPoint.position;
                if (Vector3.Dot(trackPoint.rotation * Vector3.forward, directionFromTrackPointToPlayer) < 0)
                {

                    return;
                }
                trackPointBehind = trackPoint;
            }
        }
    }

    public void CalculateInterpolatedPosition(ref Quaternion interpolatedRotation, TrackPoint trackPointBehind, Transform transform)
    {
        TrackPoint trackPointAhead = mapController.TrackPoints[trackPointBehind.index + 1];

        Vector3 trackDirection = Vector3.Dot(trackPointAhead.position - trackPointBehind.position, trackPointBehind.rotation * Vector3.forward) * (trackPointBehind.rotation * Vector3.forward);
        // changing the logic of CalculatedInterpolated so that when only going left or right, it never changes trackPointBehind, 
        // with the old logic: Vector3 trackDirection = trackPointAhead.position - trackPointBehind.position,
        // this may occur, and if trackPointBehind -1 +1 constantly, the motor change its rotation constantly, 
        // imagine going right but rotation change back and forth, that affects player experiences
        float t = Vector3.Dot(transform.position - trackPointBehind.position, trackDirection) / trackDirection.sqrMagnitude;
        t = Mathf.Clamp01(t);

        interpolatedRotation = Quaternion.Slerp(trackPointBehind.rotation, trackPointAhead.rotation, t);
        interpolatedRotation.Normalize();

        // for debug draw
        Debug.DrawLine(trackPointBehind.position + new Vector3(0, 1f, 0), trackPointAhead.position + new Vector3(0, 1f, 0), Color.blue);
        Debug.DrawLine(trackPointBehind.position + new Vector3(0, 1f, 0), trackPointBehind.position + trackDirection + new Vector3(0, 1f, 0), Color.blue);
        Vector3 interpolatedPosition = Vector3.Lerp(trackPointBehind.position, trackPointBehind.position + trackDirection, t);
        Debug.DrawLine(transform.position + new Vector3(0, 1f, 0), interpolatedPosition + new Vector3(0, 1f, 0), Color.blue);
    }

    public void Move(Quaternion interpolatedRotation, float verticalInput, float horizontalInput, bool isGrounded = true)
    {


        WorkingWithAcceleration();

        Vector3 targetVelocityForward = verticalInput * moveSpeed * (interpolatedRotation * Vector3.forward);
        Vector3 targetVelocityRight = horizontalInput * moveSpeed * (interpolatedRotation * Vector3.right);

        // rotate the current velocity based on previous and current rotation, but just use Yaw rotate (rotate around Up Y world axis)

        Vector3 prevForward = Vector3.ProjectOnPlane(previousRotation * Vector3.forward, Vector3.up).normalized;
        Vector3 currForward = Vector3.ProjectOnPlane(interpolatedRotation * Vector3.forward, Vector3.up).normalized;

        Quaternion yawRotation = Quaternion.FromToRotation(prevForward, currForward);
        //Quaternion deltaRotation = interpolatedRotation * Quaternion.Inverse(previousRotation);
        rb.velocity = yawRotation * rb.velocity;


        Vector3 currentVelocityRight;
        Vector3 currentVelocityWithoutRight;

      
        currentVelocityWithoutRight = Vector3.ProjectOnPlane(rb.velocity, interpolatedRotation * Vector3.right);
        currentVelocityRight = Vector3.Dot(rb.velocity, interpolatedRotation * Vector3.right) * (interpolatedRotation * Vector3.right);



        Vector3 speedErrorWithoutRight = targetVelocityForward - currentVelocityWithoutRight;
        Vector3 speedErrorRight = targetVelocityRight - currentVelocityRight;
        
        // addForce to reach the target velocity
        Vector3 forceWithoutRight = Vector3.zero;
        Vector3 forceRight;

        float newDecelerationForward;

        if (!isGrounded)
        {
            // if not grounded, we don't want to add force in world up direction, to avoid conflicting with gravity
            // and deceleration of forward (decelerationOnSky) should be small, if deceleration is too large, forward velocity will be canceled out, and velocity.y is still !0, making the player move upwards, that's weird
            newDecelerationForward = decelerationOnSky;
            forceWithoutRight = Vector3.ProjectOnPlane(speedErrorWithoutRight * newDecelerationForward, Vector3.up);
        }

        if (isGrounded)
        {
            if (targetVelocityForward == Vector3.zero)
            {
                newDecelerationForward = decelerationForward;
                if (Mathf.Abs(currentVelocityWithoutRight.magnitude) < 3f)
                {
                    newDecelerationForward = (rb.mass / Time.fixedDeltaTime) / 10f;
                }

                if (Mathf.Abs(currentVelocityWithoutRight.magnitude) < 0.4f)
                {
                    newDecelerationForward = rb.mass / Time.fixedDeltaTime;
                }
                forceWithoutRight = speedErrorWithoutRight * newDecelerationForward;


                float currentVelocityUp = Vector3.Dot(rb.velocity, interpolatedRotation * Vector3.up);
                if (currentVelocityUp < 0.45f) // remember consider even negative, we don't want to add force in up direction
                {
                    // don't add force in up direction any more to not conflict with wheel collider's suspension, to avoid jitter 
                    forceWithoutRight = Vector3.ProjectOnPlane(forceWithoutRight, interpolatedRotation * Vector3.up);
                }

            }
            else
            {
                forceWithoutRight = speedErrorWithoutRight * acceleration;
                // when accelerate the object, we don't add up force to avoid conflict with wheel collider's suspension, to avoid jitter while moving fast
                forceWithoutRight = Vector3.ProjectOnPlane(forceWithoutRight, interpolatedRotation * Vector3.up);
            }
        }

        if (targetVelocityRight == Vector3.zero)
        {
            forceRight = speedErrorRight * decelerationRight;
        }
        else
        {
            forceRight = speedErrorRight * acceleration;
        }



        rb.AddForce(forceWithoutRight, ForceMode.Force);
        rb.AddForce(forceRight, ForceMode.Force);

        previousRotation = interpolatedRotation;
    }

    private void WorkingWithAcceleration()
    {
        if (!acchieveTargetSpeedInstantly)
        {
            // if acceleration or deceleration surpasses rb.mass / Time.fixedDeltaTime, the player will surpass the target speed in one frame
            // that's not what we want (and this can also cause jitter issue), we will need to reduce acceleration or deceleration
            if (acceleration > rb.mass / Time.fixedDeltaTime || decelerationForward > rb.mass / Time.fixedDeltaTime || decelerationRight > rb.mass / Time.fixedDeltaTime)
            {
                Debug.LogWarning("Acceleration or deceleration is too high, it may cause jitter issue");
            }
        }
        else
        {
            acceleration = rb.mass / Time.fixedDeltaTime;
            decelerationForward = rb.mass / Time.fixedDeltaTime;
            decelerationRight = rb.mass / Time.fixedDeltaTime;
        }
    }
}
