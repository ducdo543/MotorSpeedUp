using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILobbyManager : MonoBehaviour
{
    public UILobbyManager Instance { get; private set; }

    [SerializeField] private ParticleSystem smoke;
    void Start()
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

    public void RaceOnClick()
    {
        // Load the race scene
        SceneLoader.Instance.LoadSceneCustom(SceneID.RaceScene);
    }

    public void PlaySmoke()
    {
        smoke.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        smoke.Play(true);
    }


}
