using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MotorSpeedUp.Data;

public class BikerButtonContainer : MonoBehaviour
{
    [SerializeField] private Outline outline;
    [SerializeField] private BikerType bikerType;
    public BikerType BikerType => bikerType;

    public void SetOutline(bool flag)
    {
        if (outline == null)
        {
            outline = transform.GetComponent<Outline>();
        }
        outline.enabled = flag;
    }
}
