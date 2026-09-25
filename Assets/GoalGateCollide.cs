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

    private void Awake()
    {
        //// ignore collision between this collider and the colliders in collidersToIgnore
        //thisCollider = GetComponent<Collider>();
        //foreach (Collider collider in collidersToIgnore)
        //{
        //    Physics.IgnoreCollision(thisCollider, collider);
        //}

        // get the collider of the motorController
        if (motorController != null)
        {
            motorColliders = motorController.GetComponentsInChildren<Collider>();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
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
        
    }
}
