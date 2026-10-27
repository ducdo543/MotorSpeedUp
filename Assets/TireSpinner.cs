using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TireSpinner : MonoBehaviour
{
    [SerializeField] private Transform frontTire;
    [SerializeField] private Transform backTire;

    public Transform FrontTire => frontTire;
    public Transform BackTire => backTire;

    public void RotateTires(Quaternion frontTargetRotation, Quaternion backTargetRotation)
    {
        frontTire.rotation = frontTargetRotation;
        backTire.rotation = backTargetRotation;
    }
}
