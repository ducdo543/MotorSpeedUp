using MotorSpeedUp.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScrollViewController : MonoBehaviour, IMotorBikerScrollView
{
    [SerializeField] private LobbyMapController lobbyMapController;

    [SerializeField] private MapButtonContainer[] mapButtonContainers;

    private MapButtonContainer previousMapButtonContainer;

    private void Start()
    {
        // Get the current level from DataManager and set the outline for that button
        int currentLevel = GameManager.Instance.DataManager.MotorSpeedUpData.Level;

        foreach (MapButtonContainer mapButtonContainer in mapButtonContainers)
        {
            if (mapButtonContainer.Level == currentLevel)
            {
                SetMapOnButtonClick(mapButtonContainer);
                break;
            }
        }
    }

    // Using this method for OnClick in buttons
    public void SetMapOnButtonClick(MapButtonContainer mapButtonContainer)
    {
        if (mapButtonContainer == null)
        {
            Debug.LogWarning("Haven't set a parameter.");
            return;
        }

        if (previousMapButtonContainer == mapButtonContainer)
        {
            return;
        }

        mapButtonContainer.SetOutline(true);

        // Set the new runtime data for MotorSpeedUpData if it has changed
        int newLevel = mapButtonContainer.Level;
        int currentLevel = GameManager.Instance.DataManager.MotorSpeedUpData.Level;

        if (newLevel != currentLevel)
        {
            MotorType currentMotorType = GameManager.Instance.DataManager.MotorSpeedUpData.MotorType;
            BikerType currentBikerType = GameManager.Instance.DataManager.MotorSpeedUpData.BikerType;

            GameManager.Instance.DataManager.SetDataRuntime(
                newLevel,
                currentMotorType,
                currentBikerType
            );
        }

        if (previousMapButtonContainer != null)
        {
            previousMapButtonContainer.SetOutline(false);

            // Reinitialize the lobbyMap after changing the map
            lobbyMapController.Initialize(newLevel);

            // On the first call, only set the outline.
            // LobbyInitializer.Initialize() has already been called in its Start().
        }

        previousMapButtonContainer = mapButtonContainer;
    }
}
