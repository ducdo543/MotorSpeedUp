using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapController : MonoBehaviour
{

    [SerializeField] private List<TrackPoint> trackPoints = new List<TrackPoint>();
    public List<TrackPoint> TrackPoints => trackPoints;
    private int mapID;

    public void InitializeMap(int mapID, List<TrackPoint> trackPoints)
    {
        this.mapID = mapID;
        this.trackPoints = trackPoints;
    }

    
}

[Serializable]
public struct TrackPoint
{
    public int index;
    public Vector3 position;
    public float percentage;
    public Quaternion rotation;

    public TrackPoint(int index, Vector3 position, float percentage, Quaternion rotation)
    {
            this.index = index;
            this.position = position;
            this.percentage = percentage;
            this.rotation = rotation;
    }
}
