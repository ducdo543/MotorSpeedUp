using MotorSpeedUp.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapButtonContainer : MonoBehaviour
{
    [SerializeField] private Outline outline;
    [SerializeField] private int level;
    public int Level => level;

    public void SetOutline(bool flag)
    {
        if (outline == null)
        {
            outline = transform.GetComponent<Outline>();
        }
        outline.enabled = flag;
    }
}
