using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyInitializer : MonoBehaviour
{
    [SerializeField] private MotorBikerSpawner motorBikerSpawner;
    [SerializeField] private GameObject motorHolder;

    [SerializeField] private LobbyTireRotater lobbyTireRotater;

    [Header("UI-Related")]
    [SerializeField] private UILobbyManager uiLobbyManager;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        // clear all the children of the MotorHolder first
        foreach (Transform child in motorHolder.transform)
        {
            child.gameObject.SetActive(false); // this prevents GetComponent from finding it
            Destroy(child.gameObject); // destroy only happens after the Update frame and before
                                       // rendering, so it can affect GetComponent
        }

        motorBikerSpawner.SpawnMotorBiker(motorHolder.transform);

        lobbyTireRotater.Initialize();

        // UI-Related
        // play smoke effect
        uiLobbyManager.PlaySmoke();
    }

    void Update()
    {
        lobbyTireRotater.UpdateTires();
    }
}
