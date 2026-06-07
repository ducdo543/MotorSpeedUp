using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikerController : MonoBehaviour
{
    public void ChangeTransform(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
    }
}
