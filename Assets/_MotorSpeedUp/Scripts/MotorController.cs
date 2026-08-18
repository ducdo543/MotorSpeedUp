using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorController : MonoBehaviour
{
    [SerializeField] private BikerController bikerController;

    [Header("Movement")]
    private MotorMovement motorMovement;
    void Start()
    {
        motorMovement = GetComponent<MotorMovement>();
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
        motorMovement.Move();
    }
}
