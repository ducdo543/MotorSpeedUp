using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class MotorSpeedUpData
{
    [SerializeField] private int level = 1;
    public int Level => level;

    public void SetNewData(int level)
    {
        this.level = level;
    }
}
