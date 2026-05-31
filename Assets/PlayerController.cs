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
        
        moveDirection = moveAction.action.ReadValue<Vector2>();

        Debug.Log(moveAction.action.controls.Count);
        //Debug.Log(Keyboard.current.wKey.isPressed);

        //foreach (var binding in moveAction.action.bindings)
        //{
        //    Debug.Log(binding.path);
        //}

        //Debug.Log(moveAction.action.phase);
        //Debug.Log(moveAction.action.triggered);
        //Debug.Log(moveAction.action.ReadValue<Vector2>());
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(moveDirection.x * moveSpeed, rb.velocity.y, moveDirection.y * moveSpeed);
    }

}
