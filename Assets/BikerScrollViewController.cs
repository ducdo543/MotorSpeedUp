using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MotorSpeedUp.Data;

public class BikerScrollViewController : MonoBehaviour, IMotorBikerScrollView
{
    [SerializeField] private LobbyInitializer lobbyInitializer;

    [SerializeField] private BikerButtonContainer[] bikerButtonContainers;

    private BikerButtonContainer previousBikerButtonContainer;

    private void Start()
    {
        // from data manager, we will get the current biker type and set the outline for that button
        BikerType currentBikerType = GameManager.Instance.DataManager.MotorSpeedUpData.BikerType;
        foreach (BikerButtonContainer bikerButtonContainer in bikerButtonContainers)
        {
            if (bikerButtonContainer.BikerType == currentBikerType)
            {
                SetBikerOnButtonClick(bikerButtonContainer);
                break;
            }
        }
    }


    // using this method for OnClick in buttons
    public void SetBikerOnButtonClick(BikerButtonContainer bikerButtonContainer)
    {
        if (bikerButtonContainer == null)
        {
            Debug.LogWarning($"haven't set a parameter");
            return;
        }


        if (previousBikerButtonContainer == bikerButtonContainer)
        {
            return;
        }


        bikerButtonContainer.SetOutline(true);


        

        // set the new runtime data (if has) for MotorSpeedUpData
        BikerType newBikerType = bikerButtonContainer.BikerType;
        BikerType currentBikerType = GameManager.Instance.DataManager.MotorSpeedUpData.BikerType;

        if (newBikerType != currentBikerType)
        {
            int currentLevel = GameManager.Instance.DataManager.MotorSpeedUpData.Level;
            MotorType currentMotorType = GameManager.Instance.DataManager.MotorSpeedUpData.MotorType;


            GameManager.Instance.DataManager.SetDataRuntime(currentLevel, currentMotorType, newBikerType);
        }


        if (previousBikerButtonContainer != null)
        {
            previousBikerButtonContainer.SetOutline(false);
            // after setting data for MotorSpeedUpData, we will spawn the biker again
            lobbyInitializer.Initialize();
        }
        previousBikerButtonContainer = bikerButtonContainer;

    }
}
