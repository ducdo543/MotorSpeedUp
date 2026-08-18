using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleCollisionSetup : MonoBehaviour
{
    private void Awake()
    {
        // ignore collision between all colliders in the vehicle
        Collider[] colliders = GetComponentsInChildren<Collider>();
        for (int i = 0; i < colliders.Length; i++)
        {
            for (int j = i + 1; j < colliders.Length; j++)
            {
                Physics.IgnoreCollision(colliders[i], colliders[j]);
            }
        }
    }
}
