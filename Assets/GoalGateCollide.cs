using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalGateCollide : MonoBehaviour
{
    //[Header("Ignore Collision")]
    //[SerializeField] private Collider[] collidersToIgnore;
    //private Collider thisCollider;

    [Header("Main Motor")]
    [SerializeField] private MotorController mainMotorController;
    private Collider[] mainMotorColliders;
    [SerializeField] private UIRaceManager uiRaceManager;

    [Header("AI Motor")]
    [SerializeField] private MotorController aiMotorController;
    private Collider[] aiMotorColliders;

    public void Initialize(MotorController mainMotorController, MotorController aiMotorController, UIRaceManager uiRaceManager)
    {
        this.mainMotorController = mainMotorController;
        this.aiMotorController = aiMotorController;
        this.uiRaceManager = uiRaceManager;

        //// ignore collision between this collider and the colliders in collidersToIgnore
        //thisCollider = GetComponent<Collider>();
        //foreach (Collider collider in collidersToIgnore)
        //{
        //    Physics.IgnoreCollision(thisCollider, collider);
        //}




        // get the collider of the mainMotorController
        mainMotorColliders = mainMotorController.GetComponentsInChildren<Collider>();
        if (mainMotorColliders == null || mainMotorColliders.Length == 0)
        {
            Debug.LogError("MainMotorController has no colliders in its children.");
        }

        // get the collider of the aiMotorController
        aiMotorColliders = aiMotorController.GetComponentsInChildren<Collider>();
        if (aiMotorColliders == null || aiMotorColliders.Length == 0)
        {
            Debug.LogError("AIMotorController has no colliders in its children.");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        // working with the AI motor
        foreach (Collider aiMotorCollider in aiMotorColliders)
        {
            if (other == aiMotorCollider)
            {
                //Debug.Log("AIMotorController entered the goal gate");
                // Call a method in MotorController to handle reaching the goal
                aiMotorController.SetReachedGoal(true);
                
                break; // Exit the loop once a match is found
            }
        }

        // working with the main motor
        foreach (Collider mainMotorCollider in mainMotorColliders)
        {
            if (other == mainMotorCollider)
            {
                //Debug.Log("MotorController entered the goal gate");
                // Call a method in MotorController to handle reaching the goal
                mainMotorController.SetReachedGoal(true);

                // working with the UI
                uiRaceManager.SetIsWin(true);

                break; // Exit the loop once a match is found
            }
        }

    }
}
