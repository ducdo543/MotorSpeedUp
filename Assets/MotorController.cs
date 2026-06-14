using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorController : MonoBehaviour
{
    [SerializeField] private Transform pointForBiker;
    [SerializeField] private BikerController bikerController;
    private Vector3 offSetLocal;

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
        motorMovement.GetMoveDirection();
        motorMovement.GetRotation();
        motorMovement.RotatePlayer();

        bikerController.ChangeTransform(pointForBiker);
    }

    private void FixedUpdate()
    {
        motorMovement.Move();
    }
}
