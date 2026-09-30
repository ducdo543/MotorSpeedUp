using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MotorSpeedUp.Data;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public DataManager DataManager { get; private set; }

    public MotorSpeedUpData MotorSpeedUpData => DataManager.MotorSpeedUpData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DataManager = new DataManager();
        LoadGame();
        //Debug.Log("Current Level: " + DataManager.MotorSpeedUpData.Level);
        
    }

    private void Update()
    {
        //just for testing, remove this later
        if (Input.GetKeyDown(KeyCode.O))
            {
                SaveGame(4, MotorType.Motor1, BikerType.Biker2);
                //Debug.Log("Current Level: " + DataManager.MotorSpeedUpData.Level);
            }
    }

    private void LoadGame()
    {
        DataManager.LoadData();
    }    

    public void SaveGame(int level = 1, MotorType motorType = MotorType.Motor1, BikerType bikerType = BikerType.Biker1)
    {
        DataManager.SaveGame(level, motorType, bikerType);
    }
}
