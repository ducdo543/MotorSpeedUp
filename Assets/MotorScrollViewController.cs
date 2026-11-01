using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MotorSpeedUp.Data;

public class MotorScrollViewController : MonoBehaviour
{
    [SerializeField] private LobbyInitializer lobbyInitializer;

    [SerializeField] private MotorButtonContainer[] motorButtonContainers;

    private MotorButtonContainer previousMotorButtonContainer;

    private void Start()
    {
        // from data manager, we will get the current motor type and set the outline for that button
        MotorType currentMotorType = GameManager.Instance.DataManager.MotorSpeedUpData.MotorType;
        foreach (MotorButtonContainer motorButtonContainer in motorButtonContainers)
        {
            if (motorButtonContainer.MotorType == currentMotorType)
            {
                SetMotorOnButtonClick(motorButtonContainer);
                break;
            }
        }
    }


    // using this method for OnClick in buttons
    public void SetMotorOnButtonClick(MotorButtonContainer motorButtonContainer)
    {
        if (motorButtonContainer == null)
        {
            Debug.LogWarning($"haven't set a parameter");
            return;
        }


        if (previousMotorButtonContainer == motorButtonContainer)
        {
            return;
        }


        if (previousMotorButtonContainer != null)
        {
            previousMotorButtonContainer.SetOutline(false);
        }

        motorButtonContainer.SetOutline(true);


        previousMotorButtonContainer = motorButtonContainer;

        // set the new runtime data (if has) for MotorSpeedUpData
        MotorType newMotorType = motorButtonContainer.MotorType;
        MotorType currentMotorType = GameManager.Instance.DataManager.MotorSpeedUpData.MotorType;

        if (newMotorType != currentMotorType)
        {
            int currentLevel = GameManager.Instance.DataManager.MotorSpeedUpData.Level;
            BikerType currentBikerType = GameManager.Instance.DataManager.MotorSpeedUpData.BikerType;


            GameManager.Instance.DataManager.SetDataRuntime(currentLevel, newMotorType, currentBikerType);
        }

        // after setting data for MotorSpeedUpData, we will spawn the motor biker again
        lobbyInitializer.Initialize();

    }    
}
