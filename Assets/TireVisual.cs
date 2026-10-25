using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TireVisual : MonoBehaviour
{
    [SerializeField] private WheelCollider[] wheelColliders;
    [SerializeField] private TireSpinner tireSpinner;

    private void Start()
    {
        wheelColliders = GetComponentsInChildren<WheelCollider>();
        tireSpinner = GetComponentInChildren<TireSpinner>();
    }

    private void Update()
    {
        wheelColliders[0].GetWorldPose(out Vector3 frontPos, out Quaternion frontRot);
        wheelColliders[1].GetWorldPose(out Vector3 backPos, out Quaternion backRot);

        tireSpinner.RotateTiers(frontRot, backRot);
    }
}
