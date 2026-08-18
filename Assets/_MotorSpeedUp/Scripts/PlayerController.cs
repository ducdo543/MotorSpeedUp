using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerMovement playerMovement;
    


    //[Header("Other serialized fields")]
    

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();

    }
    // Update is called once per frame
    void Update()
    {
        playerMovement.CalculateInterpolatedPosition();
        playerMovement.WorkingWithInput();
        playerMovement.GetRotation();
        playerMovement.RotatePlayer();
    }


    private void FixedUpdate()
    {
        playerMovement.Move();
    }
}
