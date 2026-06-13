using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandleMovement : MonoBehaviour
{
    private PlayerControls playerControls;

    private Vector2 movementInput;
    private float verticalInput;
    public float VerticalInput => verticalInput;
    private float horizontalInput;
    public float HorizontalInput => horizontalInput;

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();
        }

        playerControls.GamePlay.Move.performed += ctx => movementInput = ctx.ReadValue<Vector2>();

        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Update()
    {
        HandleMovementInput();
    }

    public void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;
    }
}
