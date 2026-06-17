using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTargetController : MonoBehaviour
{
    private Transform motor;
    [SerializeField] private float cameraDownFactor = 0.1f;
    private void Awake()
    {
        motor = GameObject.FindWithTag("MainMotor").transform;
    }

    private void FixedUpdate()
    {
        if (motor != null)
        {
            transform.position = motor.position;
            
            UpdateRotation();
        }
    }

    private void UpdateRotation()
    {
        Vector3 forward = motor.forward;
        forward.y = -(cameraDownFactor); // Add a downward component to the forward vector
        forward.Normalize();
        if (forward.sqrMagnitude > 0) // Quaternion.LookRotation requires a non-zero vector
        {
            transform.rotation = Quaternion.LookRotation(forward);
        }
    }
}
