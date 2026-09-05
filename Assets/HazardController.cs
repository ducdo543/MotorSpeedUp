using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardController : MonoBehaviour
{
    private float delay = 0f;
    [SerializeField] private List<HazardMotion> hazardMotions = new List<HazardMotion>();

    private void Start()
    {
        // random delay
        delay = Random.Range(0f, 2f);

        foreach (var hazardMotion in hazardMotions)
        {
            hazardMotion.StartMotion(delay);
        }
    }
}

