using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorController : MonoBehaviour
{
    [SerializeField] private BikerController bikerController;

    [Header("Movement")]
    private MotorMovement motorMovement;
    private VehicleRevive vehicleRevive;
    void Start()
    {
        motorMovement = GetComponent<MotorMovement>();
        vehicleRevive = GetComponent<VehicleRevive>();
    }

    // Update is called once per frame
    void Update()
    {

        motorMovement.CalculateInterpolatedPosition();

        // vehicleRevive
        if (vehicleRevive.CheckDead())
        {
            vehicleRevive.Revive(motorMovement.FurthestTrackPoint);
        }

        motorMovement.WorkingWithInput();
        motorMovement.GetRotation();
        motorMovement.RotatePlayer();

    }

    private void FixedUpdate()
    {
        motorMovement.Move();
    }
}
