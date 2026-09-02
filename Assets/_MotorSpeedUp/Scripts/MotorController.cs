using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorController : MonoBehaviour
{
    [SerializeField] private BikerController bikerController;

    [Header("Movement")]
    private MotorMovement motorMovement;
    private VehicleRevive vehicleRevive;

    //private bool dead = false;
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


        //if (vehicleRevive.CheckDead())
        //{
        //    dead = true;
        //}
    }

    private void FixedUpdate()
    {
        //// check revive just after rotating everything
        //// every physics and position update is in FixedUpdate, so we should call Revive() here
        //// vehicleRevive
        //if (dead)
        //{
        //    vehicleRevive.Revive();
        //    Debug.Log("Vehicle revived");
        //    dead = false;
        //}

        motorMovement.Move();
    }
}
