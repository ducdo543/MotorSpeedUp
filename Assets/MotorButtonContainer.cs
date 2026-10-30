using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MotorSpeedUp.Data;

public class MotorButtonContainer : MonoBehaviour
{
    [SerializeField] private Outline outline;
    [SerializeField] private MotorType motorType;
    public MotorType MotorType => motorType;
    
    public void SetOutline(bool flag)
    {
        if (outline == null)
        {
            outline = transform.GetComponent<Outline>();
        }
        outline.enabled = flag;
    }    
}
