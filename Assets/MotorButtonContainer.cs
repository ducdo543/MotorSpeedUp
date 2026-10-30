using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MotorButtonContainer : MonoBehaviour
{
    [SerializeField] Outline outline;

    public void SetMainMotor()
    {
        if (outline == null)
        {
            outline = transform.GetComponent<Outline>();
        }

        SetOutline(true);

    }   
    
    public void SetOutline(bool flag)
    {
        outline.enabled = flag;
    }    
}
