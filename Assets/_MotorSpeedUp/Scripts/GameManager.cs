using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public DataManager DataManager { get; private set; }

    private int currentLevel = 1;
    public int CurrentLevel => currentLevel;

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
        currentLevel = DataManager.MotorSpeedUpData.Level;
    }

    private void Update()
    {
        // just for testing, remove this later
        if (Input.GetKeyDown(KeyCode.O))
        {
            SaveGame(2);
            //Debug.Log("Current Level: " + DataManager.MotorSpeedUpData.Level);
        }
    }

    private void LoadGame()
    {
        DataManager.LoadData();
    }    

    public void SaveGame(int level)
    {
        DataManager.SaveGame(level);
    }
}
