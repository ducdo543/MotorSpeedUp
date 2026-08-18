using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager
{
    private string savePath = Application.persistentDataPath + "/MotorSpeedUpData.json";
    private MotorSpeedUpData motorSpeedUpData;
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
    public void SaveGame(int level)
    {
        motorSpeedUpData.SetNewData(level);
        string json = JsonUtility.ToJson(motorSpeedUpData);
        File.WriteAllText(savePath, json);
    }
}
