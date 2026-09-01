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

        vehicleRevive.Initialize(motorMovement);
    }

    // Update is called once per frame
    void Update()
    {

        motorMovement.CalculateInterpolatedPosition();

        motorMovement.WorkingWithInput();
        motorMovement.GetRotation();
        motorMovement.RotatePlayer();

    }

    private void FixedUpdate()
    {
        // check revive just after rotating everything
        // every physics and position update is in FixedUpdate, so we should call Revive() here
        // vehicleRevive
        if (vehicleRevive.CheckDead())
        {
            vehicleRevive.Revive();
        }

        motorMovement.Move();
    }
}
