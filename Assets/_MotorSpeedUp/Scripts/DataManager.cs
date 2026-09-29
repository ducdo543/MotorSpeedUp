using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using MotorSpeedUp.Data;

// use DataManager to save and load data from json file
namespace MotorSpeedUp.Data
{
    public class DataManager
    {
        private string savePath = Application.persistentDataPath + "/MotorSpeedUpData.json";
        private MotorSpeedUpData motorSpeedUpData = new MotorSpeedUpData();
        public MotorSpeedUpData MotorSpeedUpData => motorSpeedUpData;

        public void LoadData()
        {
            motorSpeedUpData = new MotorSpeedUpData();
            if (File.Exists(savePath))
            {
                string json = File.ReadAllText(savePath);
                motorSpeedUpData = JsonUtility.FromJson<MotorSpeedUpData>(json);
            }
            else
            {
                Debug.Log("Save file not found, creating new data.");
            }
        }
        public void SaveGame(int level = 1, int motorType = 1, BikerType bikerType = BikerType.Biker1)
        {
            motorSpeedUpData.SetNewData(level, motorType, bikerType);
            string json = JsonUtility.ToJson(motorSpeedUpData);
            File.WriteAllText(savePath, json);
        }
    }
}
