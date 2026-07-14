using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVehicleMovement
{
    void WorkingWithInput();
    void Move();

    void GetRotation();

    void RotatePlayer();

    void CalculateInterpolatedPosition();
    bool IsGrounded();
}
