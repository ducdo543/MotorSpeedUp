using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MotorSpeedUp.Data;

public class MotorScrollViewController : MonoBehaviour
{
    [SerializeField] private LobbyInitializer lobbyInitializer;

    private MotorButtonContainer previousMotorButtonContainer;




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

        // set the runtime data for MotorSpeedUpData
        int currentLevel = GameManager.Instance.DataManager.MotorSpeedUpData.Level;
        BikerType currentBikerType = GameManager.Instance.DataManager.MotorSpeedUpData.BikerType;
        MotorType newMotorType = motorButtonContainer.MotorType;

        GameManager.Instance.DataManager.SetDataRuntime(currentLevel, newMotorType, currentBikerType);

        // after setting data for MotorSpeedUpData, we will spawn the motor biker again
        lobbyInitializer.Initialize();

    }    
}
