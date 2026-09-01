using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVehicleMovement
{
    public Quaternion InterpolatedRotation { get; }

    public BaseMoveOnSpline BaseMoveOnSpline { get; }

    public TrackPoint FurthestTrackPoint { get; }

    void ResetVehicleFields(TrackPoint trackPoint);
    void WorkingWithInput();
    void Move();

    void GetRotation();

    void RotatePlayer();

    void CalculateInterpolatedPosition();
    bool IsGrounded();
}
