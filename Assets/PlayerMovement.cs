using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private InputHandlePlayer inputHandlePlayer;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private Rigidbody rb;
    private Vector3 playerDirection = new Vector3();
    private float pastYVelocity = 0f;

    [Header("Other serialized fields")]
    [SerializeField] private Transform cameraTransform;
    

    private void Start()
    {
        inputHandlePlayer = GetComponent<InputHandlePlayer>();

    }

    public void GetNewDirection()
    {
        inputHandlePlayer.HandleMovementInput();
        playerDirection = cameraTransform.forward * inputHandlePlayer.VerticalInput + cameraTransform.right * inputHandlePlayer.HorizontalInput;
        playerDirection.Normalize();
    }

    public void MovePlayer()
    {
        if (rb.velocity.y == 0)
        {
            pastYVelocity = 0f;
        }
        float currentYVelocity = rb.velocity.y - pastYVelocity + playerDirection.y * moveSpeed;
        rb.velocity = new Vector3(playerDirection.x * moveSpeed, currentYVelocity, playerDirection.z * moveSpeed);
        pastYVelocity = rb.velocity.y;
    }

    public void RotatePlayer()
    {
        if (playerDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(playerDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    
   
}