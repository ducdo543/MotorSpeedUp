using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVehicleMovementRaw
{
    public Quaternion InterpolatedRotation { get; }

    public BaseMoveOnSpline BaseMoveOnSpline { get; }

    public TrackPoint FurthestTrackPoint { get; }

    void WorkingWithInput();

    void SetNoInput();
    void ResetVehicleFields(TrackPoint trackPoint);
    
    void Move();

    void RotatePlayer();

    void CalculateInterpolatedPosition();
    bool IsGrounded();
}
