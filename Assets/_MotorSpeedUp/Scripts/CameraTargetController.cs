using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraTargetController : MonoBehaviour
{
    [SerializeField] private Transform vehicle;
    private IVehicleMovement vehicleMovement;
    private VehicleRevive vehicleRevive;
    public Quaternion InterpolatedRotation => vehicleMovement.InterpolatedRotation;

    [SerializeField] private float cameraDownFactorOnGround = 0.1f;
    [SerializeField] private float cameraDownFactorOnSky = 0.5f;
    [SerializeField] private float rotationSpeedYaw = 7f;
    [SerializeField] private float rotationSpeedTilt = 3f;
    private Quaternion targetRotation;

    [Header("Virtual Camera")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    private CinemachineTransposer transposer;
    [SerializeField] private float zyDamping = 0.3f;
    [SerializeField] private float zyDampingChangeSpeed = 1f;

    private BaseMoveOnSpline baseMoveOnSpline;
    private float velocityForwardWorld;
    private void Awake()
    {
        Initialized();

        transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
    }

    private void OnValidate()
    {
        Initialized();
    }

    private void OnEnable()
    {
        vehicleRevive.OnAfterRevive += ResetFields;
    }

    private void OnDisable()
    {
        vehicleRevive.OnAfterRevive -= ResetFields;
    }
    private void Initialized()
    {
        if (vehicle == null)
        {
            vehicle = GameObject.FindWithTag("MainMotor").transform;
        }
        vehicleMovement = vehicle.GetComponent<IVehicleMovement>();
        vehicleRevive = vehicle.GetComponent<VehicleRevive>();
        baseMoveOnSpline = vehicleMovement.BaseMoveOnSpline;
    }

    private void FixedUpdate()
    {
        if (vehicle != null)
        {
            transform.position = vehicle.position;

            UpdateRotation();
        }

        UpdateDamping();
    }

    private void UpdateDamping()
    {
        // update the damping of the virtual camera based on whether the vehicle is grounded or in the air and the direction
        float targetDamping = transposer.m_ZDamping;
        //if (vehicleMovement.IsGrounded())
        //{
        //    targetDamping = damping;
        //}
        //else
        //{
        //    targetDamping = dampingInAir;
        //}

        velocityForwardWorld = baseMoveOnSpline.GetVelocityForwardWorld();

        if (velocityForwardWorld > 0.1f)
        {
            targetDamping = zyDamping;
        }
        else if (velocityForwardWorld < -0.1f)
        {
            targetDamping = 0f;
        }


        //transposer.m_XDamping = Mathf.MoveTowards(
        //    transposer.m_XDamping,
        //    targetDamping,
        //    dampingChangeSpeed * Time.fixedDeltaTime);
        transposer.m_YDamping = Mathf.MoveTowards(
            transposer.m_YDamping,
            targetDamping,
            zyDampingChangeSpeed * Time.fixedDeltaTime);
        transposer.m_ZDamping = Mathf.MoveTowards(
            transposer.m_ZDamping,
            targetDamping,
            zyDampingChangeSpeed * Time.fixedDeltaTime);
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
            Quaternion targetTilt = Quaternion.Inverse(targetYaw) * targetRotation; // targetTilt * targetYaw = targetRotation

            Vector3 currentFlatForward = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
            Quaternion currentYaw = Quaternion.LookRotation(currentFlatForward, Vector3.up);
            Quaternion currentTilt = Quaternion.Inverse(currentYaw) * transform.rotation;

            transform.rotation = Quaternion.Slerp(currentYaw, targetYaw, rotationSpeedYaw * Time.fixedDeltaTime) * Quaternion.Slerp(currentTilt, targetTilt, rotationSpeedTilt * Time.fixedDeltaTime);
        }
    }

    private void ResetFields(Quaternion rotation)
    {
        transform.rotation = rotation;

        transform.position = vehicle.position;
        transposer.m_YDamping = 0f;
        transposer.m_ZDamping = 0f;
    }
}
