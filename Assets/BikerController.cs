using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikerController : MonoBehaviour
{
    public void ChangeTransform(Transform pointForBiker)
    {
        transform.position = pointForBiker.position;
        transform.rotation = pointForBiker.rotation;
    }
}
