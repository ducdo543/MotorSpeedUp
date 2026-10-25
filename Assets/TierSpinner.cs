using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TierSpinner : MonoBehaviour
{
    [SerializeField] private Transform frontTier;
    [SerializeField] private Transform backTier;

    public void RotateTiers(Quaternion frontTargetRotation, Quaternion backTargetRotation)
    {
        frontTier.rotation = frontTargetRotation;
        backTier.rotation = backTargetRotation;
    }
}
