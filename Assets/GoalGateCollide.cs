using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalGateCollide : MonoBehaviour
{
    //[Header("Ignore Collision")]
    //[SerializeField] private Collider[] collidersToIgnore;
    //private Collider thisCollider;

    [Header("Other SerializeFields")]
    [SerializeField] private MotorController motorController;
    private Collider[] motorColliders;
    [SerializeField] private UIRaceManager uiRaceManager;

    public void Initialize(MotorController motorController, UIRaceManager uiRaceManager)
    {
        this.motorController = motorController;
        this.uiRaceManager = uiRaceManager;

        //// ignore collision between this collider and the colliders in collidersToIgnore
        //thisCollider = GetComponent<Collider>();
        //foreach (Collider collider in collidersToIgnore)
        //{
        //    Physics.IgnoreCollision(thisCollider, collider);
        //}




        // get the collider of the motorController
        motorColliders = motorController.GetComponentsInChildren<Collider>();
        if (motorColliders == null || motorColliders.Length == 0)
        {
            Debug.LogError("MotorController has no colliders in its children.");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        // working with the motor
        foreach (Collider motorCollider in motorColliders)
        {
            if (other == motorCollider)
            {
                //Debug.Log("MotorController entered the goal gate");
                // Call a method in MotorController to handle reaching the goal
                motorController.SetReachedGoal(true);
                break; // Exit the loop once a match is found
            }
        }

        // working with the UI
        uiRaceManager.SetIsWin(true);
    }
}
