using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMotorMovement : IVehicleMovementRaw
{
    public void Initialize(MapController mapController, 
        Rigidbody rb, 
        WheelCollider[] wheelColliders);
}
