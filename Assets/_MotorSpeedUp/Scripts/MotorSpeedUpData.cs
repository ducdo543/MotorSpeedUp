using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace MotorSpeedUp.Data
{
    [Serializable]
    public class MotorSpeedUpData
    {
        [SerializeField] private int level = 1;
        [SerializeField] private int motorType = 1;
        [SerializeField] private BikerType bikerType = BikerType.Biker1;
        public int Level => level;

        public void SetNewData(int level = 1, int motorType = 1, BikerType bikerType = BikerType.Biker1)
        {
            this.level = level;
            this.motorType = motorType;
            this.bikerType = bikerType;
        }
    }

    public enum BikerType
    {
        Biker1 = 1,
        Biker2 = 2,
        Biker3 = 3,
        Biker4 = 4,
        Biker5 = 5,
        Biker6 = 6,
        Biker7 = 7,
        Biker8 = 8,
        Biker9 = 9,
        Biker10 = 10
    }
}
