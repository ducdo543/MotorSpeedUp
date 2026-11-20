using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorController : MonoBehaviour
{
    //[SerializeField] private BikerController bikerController;
    [SerializeField] private GameObject map;

    [Header("Movement")]
    private IMotorMovement targetMotorMovement;
    private VehicleRevive vehicleRevive;
    private bool reachedGoal = false;
    private bool isTicking = false;
    private bool isStopping = false;
    private Rigidbody rb;
    private WheelCollider[] wheelColliders;
    private Coroutine motorRoutine;

    //private bool dead = false;
    public void Initialize(GameObject map)
    {
        rb = GetComponent<Rigidbody>();

        wheelColliders = GetComponentsInChildren<WheelCollider>();

        targetMotorMovement = GetComponent<IMotorMovement>();
        vehicleRevive = GetComponent<VehicleRevive>();


        vehicleRevive.Initialize(targetMotorMovement);

        

        RegisterForChildren(map);

        motorRoutine = StartCoroutine(MotorUpdate());
    }

    // Update is called once per frame
    IEnumerator MotorUpdate()
    {
        yield return null; // wait for one frame to ensure all child components are initialized in the Start() method (if they have) before proceeding

        while (true)
        {
            targetMotorMovement.CalculateInterpolatedPosition();

            if (reachedGoal)
            {
                targetMotorMovement.SetNoInput();
                if (!isTicking )
                {
                    isTicking = true;
                    StartCoroutine(StopAfterDelay());
                }
            }
            else
            {
                targetMotorMovement.WorkingWithInput();
            }

            targetMotorMovement.RotatePlayer();


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

        targetMotorMovement.Move();
    }

    public void SetReachedGoal(bool value)
    {
        reachedGoal = value;
    }

    private void RegisterForChildren(GameObject map)
    {
        this.map = map;
        targetMotorMovement.Initialize(map.GetComponent<MapController>(), rb, wheelColliders);
        vehicleRevive.InitializeMap(map.transform);
    }    
}
