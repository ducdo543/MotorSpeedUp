using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private InputHandlePlayer inputHandlePlayer;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Rigidbody rb;
    private Vector2 moveDirection;
    private Vector3 playerDirection = new Vector3();

    [Header("Other serialized fields")]
    [SerializeField] private Transform cameraTransform;

    private void Start()
    {
        inputHandlePlayer = GetComponent<InputHandlePlayer>();
        
    }
    // Update is called once per frame
    void Update()
    {
        
        GetNewDirection();

    }

    private void GetNewDirection()
    {
        inputHandlePlayer.HandleMovementInput();
        playerDirection = transform.forward * inputHandlePlayer.VerticalInput + transform.right * inputHandlePlayer.HorizontalInput;
        playerDirection.y = 0f;
        playerDirection.Normalize();


    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector3(playerDirection.x * moveSpeed, rb.velocity.y, playerDirection.z * moveSpeed);
        Debug.Log(playerDirection.y * moveSpeed);
    }

}
