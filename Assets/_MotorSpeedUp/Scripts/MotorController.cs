using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorController : MonoBehaviour
{
    [SerializeField] private BikerController bikerController;

    [Header("Movement")]
    private MotorMovement motorMovement;
    private VehicleRevive vehicleRevive;
    private bool reachedGoal = false;
    private bool isTicking = false;
    private bool isStopping = false;
    private Rigidbody rb;
    private Coroutine motorRoutine;

    //private bool dead = false;
    void Start()
    {
        motorMovement = GetComponent<MotorMovement>();
        vehicleRevive = GetComponent<VehicleRevive>();

        vehicleRevive.Initialize(motorMovement);

        rb = GetComponent<Rigidbody>();

        motorRoutine = StartCoroutine(MotorUpdate());
    }

    // Update is called once per frame
    IEnumerator MotorUpdate()
    {
        while (true)
        {
            motorMovement.CalculateInterpolatedPosition();

            if (reachedGoal)
            {
                motorMovement.SetNoInput();
                if (!isTicking )
                {
                    isTicking = true;
                    StartCoroutine(StopAfterDelay());
                }
            }
            else
            {
                motorMovement.WorkingWithInput();
            }

            motorMovement.GetRotation();
            motorMovement.RotatePlayer();


            //if (vehicleRevive.CheckDead())
            //{
            //    dead = true;
            //}
            yield return null;
        }
    }

    IEnumerator StopAfterDelay()
    {
        yield return new WaitForSeconds(1.5f);
        yield return new WaitForFixedUpdate();// ensure this happens in physics step

        rb.isKinematic = true;

        StopCoroutine(motorRoutine);

        isStopping = true;
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

        if (isStopping)
        {
            return;
        }

        motorMovement.Move();
    }

    public void SetReachedGoal(bool value)
    {
        reachedGoal = value;
    }

}
