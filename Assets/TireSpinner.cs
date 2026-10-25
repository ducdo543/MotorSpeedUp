using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TireSpinner : MonoBehaviour
{
    [SerializeField] private Transform frontTire;
    [SerializeField] private Transform backTire;

    public void RotateTiers(Quaternion frontTargetRotation, Quaternion backTargetRotation)
    {
        frontTire.rotation = frontTargetRotation;
        backTire.rotation = backTargetRotation;
    }
}
