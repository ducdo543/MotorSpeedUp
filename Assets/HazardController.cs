using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardController : MonoBehaviour
{
    [SerializeField] private bool canDead = false;
    public bool CanDead => canDead;
}
