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
        [SerializeField] private MotorType motorType;
        [SerializeField] private BikerType bikerType = BikerType.Biker1;
        public int Level => level;
        public MotorType MotorType => motorType;
        public BikerType BikerType => bikerType;

        public void SetNewData(int level = 1, MotorType motorType = MotorType.Motor1, BikerType bikerType = BikerType.Biker1)
        {
            this.level = level;
            this.motorType = motorType;
            this.bikerType = bikerType;
        }
    }

    public enum MotorType
    {
        Motor1 = 1,
        Motor2 = 2,
        Motor3 = 3,
        Motor4 = 4,
        Motor5 = 5,
        Motor6 = 6,
        Motor7 = 7,
        Motor8 = 8,
        Motor9 = 9,
        Motor10 = 10,
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
