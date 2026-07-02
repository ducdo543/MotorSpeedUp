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
    [SerializeField] private Rigidbody rb;
    private Quaternion previousRotation;
    public void SetFields(MapController mapController, Rigidbody rb)
    {
        this.mapController = mapController;
        this.rb = rb;
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
        Vector3 trackDirection = (trackPointAhead.position - trackPointBehind.position);
        float t = Vector3.Dot(transform.position - trackPointBehind.position, trackDirection) / trackDirection.sqrMagnitude;
        t = Mathf.Clamp01(t);

        interpolatedRotation = Quaternion.Slerp(trackPointBehind.rotation, trackPointAhead.rotation, t);
        interpolatedRotation.Normalize();

        // for debug draw
        Debug.DrawLine(trackPointBehind.position, trackPointAhead.position, Color.red);
        Vector3 interpolatedPosition = Vector3.Lerp(trackPointBehind.position, trackPointAhead.position, t);
        Debug.DrawLine(transform.position, interpolatedPosition, Color.green);
    }

    public void Move(Quaternion interpolatedRotation, float verticalInput, float horizontalInput)
    {
        // discard
        //// we shouldn't accelerate gradually on each axis separately, so I discard this code as commented.
        //float targetZVelocity = playerMoveDirection.z * moveSpeed;
        //currentZVelocity = Mathf.MoveTowards(rb.velocity.z, targetZVelocity, acceleration * Time.fixedDeltaTime);
        //float targetXVelocity = playerMoveDirection.x * moveSpeed;
        //currentXVelocity = Mathf.MoveTowards(rb.velocity.x, targetXVelocity, acceleration * Time.fixedDeltaTime);
        //currentZVelocity = playerMoveDirection.z * moveSpeed;
        //currentXVelocity = playerMoveDirection.x * moveSpeed;

        // discard
        //// instead of set rb.velocity directly, we can use add force
        //Vector3 currentVelocity = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z);
        //currentVelocity = Vector3.MoveTowards(currentVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
        //rb.velocity = new Vector3(currentVelocity.x, currentVelocity.y, currentVelocity.z);



        WorkingWithAcceleration();

        float targetSpeedForward = verticalInput * moveSpeed;
        float targetSpeedRight = horizontalInput * moveSpeed;

        // rotate the current velocity based on previous and current rotation, but just use Yaw rotate (rotate around Up Y world axis)

        Vector3 prevForward = Vector3.ProjectOnPlane(previousRotation * Vector3.forward, Vector3.up).normalized;
        Vector3 currForward = Vector3.ProjectOnPlane(interpolatedRotation * Vector3.forward, Vector3.up).normalized;

        Quaternion yawRotation = Quaternion.FromToRotation(prevForward, currForward);
        //Quaternion deltaRotation = interpolatedRotation * Quaternion.Inverse(previousRotation);
        rb.velocity = yawRotation * rb.velocity;

        // when calculate force, projecting the vector velocity onto a road plane, we want to remove the normal force so that it doesn't affect spring force of wheelCollider
        Vector3 projectedCurrentVelocity = Vector3.ProjectOnPlane(rb.velocity, interpolatedRotation * Vector3.up);
       
        float currentSpeedForward = Vector3.Dot(projectedCurrentVelocity, interpolatedRotation * Vector3.forward);
        float currentSpeedRight = Vector3.Dot(projectedCurrentVelocity, interpolatedRotation * Vector3.right);
        float speedErrorForward = targetSpeedForward - currentSpeedForward;
        float speedErrorRight = targetSpeedRight - currentSpeedRight;
        // addForce to reach the target velocity
        Vector3 forceForward;
        Vector3 forceRight;

        if (targetSpeedForward == 0)
        {
            float newDecelerationForward = decelerationForward;
            if (Mathf.Abs(currentSpeedForward) < 3f)
            {
                newDecelerationForward = (rb.mass / Time.fixedDeltaTime)/10f;
            }

            if (Mathf.Abs(currentSpeedForward) < 0.4f)
            {
                newDecelerationForward = rb.mass / Time.fixedDeltaTime;
            }
            forceForward = speedErrorForward * newDecelerationForward * (interpolatedRotation * Vector3.forward);
        }
        else
        {
            forceForward = speedErrorForward * acceleration * (interpolatedRotation * Vector3.forward);
        }

        if (targetSpeedRight == 0)
        {
            forceRight = speedErrorRight * decelerationRight * (interpolatedRotation * Vector3.right);
        }
        else
        {
            forceRight = speedErrorRight * acceleration * (interpolatedRotation * Vector3.right);
        }


        rb.AddForce(forceForward, ForceMode.Force);
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
