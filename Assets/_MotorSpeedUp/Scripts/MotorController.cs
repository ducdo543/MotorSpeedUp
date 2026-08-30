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


        // check revive just after rotating everything
        // vehicleRevive
        if (vehicleRevive.CheckDead())
        {
            vehicleRevive.Revive();
        }
    }

    private void FixedUpdate()
    {
        motorMovement.Move();
    }
}
