using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTargetController : MonoBehaviour
{
    [SerializeField] private Transform vehicle;
    private IVehicleMovement vehicleMovement;
    public Quaternion InterpolatedRotation => vehicleMovement.InterpolatedRotation;
    [SerializeField] private float cameraDownFactorOnGround = 0.1f;
    [SerializeField] private float cameraDownFactorOnSky = 0.5f;
    [SerializeField] private float rotationSpeedYaw = 7f;
    [SerializeField] private float rotationSpeedTilt = 3f;
    private Quaternion targetRotation;

    private void Awake()
    {
        Initialized();
    }

    private void OnValidate()
    {
        Initialized();
    }

    private void Initialized()
    {
        if (vehicle == null)
        {
            vehicle = GameObject.FindWithTag("MainMotor").transform;
        }
        vehicleMovement = vehicle.GetComponent<IVehicleMovement>();
    }

    private void FixedUpdate()
    {
        if (vehicle != null)
        {
            transform.position = vehicle.position;
            
            UpdateRotation();
        }
    }

    private void UpdateRotation()
    {
        Vector3 forward = Vector3.zero;
        if (vehicleMovement.IsGrounded())
        {
            forward = InterpolatedRotation * Vector3.forward;
            forward.y = forward.y - cameraDownFactorOnGround; // Add a downward component to the forward vector

        }
        else
        {
            forward = Vector3.ProjectOnPlane(InterpolatedRotation * Vector3.forward, Vector3.up);
            forward.Normalize();
            forward.y = -cameraDownFactorOnSky; // Add a downward component to the forward vector

        }

        forward.Normalize();
        if (forward.sqrMagnitude > 0) // Quaternion.LookRotation requires a non-zero vector
        {
            targetRotation = Quaternion.LookRotation(forward);

            // i want Slerp rotation different for tilt and yaw quaternion, so we need to detach quaternion into tilt and yaw components
            Vector3 flatForward = Vector3.ProjectOnPlane(forward, Vector3.up).normalized;
            Quaternion targetYaw = Quaternion.LookRotation(flatForward, Vector3.up);
            Quaternion targetTilt = Quaternion.Inverse(targetYaw) * targetRotation;

            Vector3 currentFlatForward = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
            Quaternion currentYaw = Quaternion.LookRotation(currentFlatForward, Vector3.up);
            Quaternion currentTilt = Quaternion.Inverse(currentYaw) * transform.rotation;

            transform.rotation = Quaternion.Slerp(currentYaw, targetYaw, rotationSpeedYaw * Time.fixedDeltaTime) * Quaternion.Slerp(currentTilt, targetTilt, rotationSpeedTilt * Time.fixedDeltaTime);
        }
    }
}
