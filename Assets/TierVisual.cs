using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TierVisual : MonoBehaviour
{
    [SerializeField] private WheelCollider[] wheelColliders;
    [SerializeField] private TierSpinner tierSpinner;

    private void Start()
    {
        wheelColliders = GetComponentsInChildren<WheelCollider>();
        tierSpinner = GetComponentInChildren<TierSpinner>();
    }

    private void Update()
    {
        wheelColliders[0].GetWorldPose(out Vector3 frontPos, out Quaternion frontRot);
        wheelColliders[1].GetWorldPose(out Vector3 backPos, out Quaternion backRot);

        tierSpinner.RotateTiers(frontRot, backRot);
    }
}
