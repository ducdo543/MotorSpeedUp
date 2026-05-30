using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Rigidbody rb;
    private Vector2 moveDirection;

    [SerializeField] private InputActionReference moveAction;

    // Update is called once per frame
    void Update()
    {
        Debug.Log(moveAction.action.enabled);
        moveDirection = moveAction.action.ReadValue<Vector2>();

    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(moveDirection.x * moveSpeed, rb.velocity.y, moveDirection.y * moveSpeed);
    }

}
