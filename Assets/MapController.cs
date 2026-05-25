using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapController : MonoBehaviour
{
    
}

[Serializable]
public struct TrackPoint
{
    public int index;
    public Vector3 position;
    public float percentage;
    public Quaternion rotation;
    public bool isAbyss;

    public TrackPoint(int index, Vector3 position, float percentage, Quaternion rotation, bool isAbyss = false)
    {
            this.index = index;
            this.position = position;
            this.percentage = percentage;
            this.rotation = rotation;
            this.isAbyss = isAbyss;
    }
}
