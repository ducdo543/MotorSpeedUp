using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILobbyManager : MonoBehaviour
{
    public UILobbyManager Instance { get; private set; }
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


}
