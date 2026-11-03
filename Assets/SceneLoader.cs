using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    private Scene currentScene;

    private AsyncOperation loadOp;

    [SerializeField] private Camera mainCamera;

    [Header("Fade settings")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeTime = 0.5f;


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
        // set fadeImage to black and transparent
        fadeImage.color = new Color(0, 0, 0, 0);
        // deactivate fadeImage
        fadeImage.gameObject.SetActive(false);
    }


    public void LoadSceneCustom(SceneID newSceneID)
    {
        // kill DOTween animations
        DOTween.KillAll(true);


        // fade in
        fadeImage.gameObject.SetActive(true);
        fadeImage.DOFade(1f, fadeTime).OnComplete(() =>
        {
            LoadNewScene(newSceneID, () =>
            {
                // turn fadeImage to black and transparent and deactivate it
                fadeImage.color = new Color(0, 0, 0, 0);
                fadeImage.gameObject.SetActive(false);
            });

        });
    }

    private void LoadNewScene(SceneID newSceneID, Action onLoadCompleted)
    {
        string newSceneName = IsEqualSceneName(newSceneID);
        // get currentScene if it's not assigned
        if (!currentScene.IsValid())
        {
            currentScene = SceneManager.GetActiveScene();
        }

        // set active scene to PersistentScene temporarily to avoid issues when unloading current scene
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("PersistentScene"));
        // activate main camera if it's not active
        mainCamera.gameObject.SetActive(true);

        // unload current scene if it's not PersistentScene
        if (currentScene.name != "PersistentScene")
        {
            SceneManager.UnloadSceneAsync(currentScene)
                .completed += (unloadOp) =>
                {
                    // load new scene additively
                    SceneManager.LoadSceneAsync(newSceneName, LoadSceneMode.Additive)
                    .completed += (op) =>
                        {
                            Scene newScene = SceneManager.GetSceneByName(newSceneName);
                            // set new scene as active scene
                            SceneManager.SetActiveScene(newScene);
                            //Debugger.LogAllLoadedScenes();
                            // update current scene
                            currentScene = newScene;
                            // deactivate main camera of persistent scene 
                            mainCamera.gameObject.SetActive(false);

                            onLoadCompleted?.Invoke();
                        };
                };
        }
    }    

    public AsyncOperation LoadFirstSceneCustom(SceneID newSceneID)
    {
        string newSceneName = IsEqualSceneName(newSceneID);

        // we are currently in PersistentScene, just load first scene additively and set it as active scene

        // load first scene additively
        var loadOp = SceneManager.LoadSceneAsync(newSceneName, LoadSceneMode.Additive);
        loadOp.completed += (op) =>
        {
            Scene newScene = SceneManager.GetSceneByName(newSceneName);
            // set new scene as active scene
            SceneManager.SetActiveScene(newScene);
            //Debugger.LogAllLoadedScenes();
            // update current scene
            currentScene = newScene;

            //Debug.Log($"After: {RenderSettings.ambientIntensity}");
            //Debug.Log($"After skybox: {RenderSettings.skybox}");

            // deactivate main camera of persistent scene
            mainCamera.gameObject.SetActive(false);
        };
        return loadOp;
    }

    private string IsEqualSceneName(SceneID sceneID)
    {
        switch (sceneID)
        {
            case SceneID.PersistentScene:
                return "PersistentScene";
            case SceneID.LobbyScene:
                return "LobbyScene";
            case SceneID.RaceScene:
                return "RaceScene";
        }

        Debug.LogError("SceneID " + sceneID.ToString() + " does not have a corresponding scene name.");
        return "";
    }
}

public enum SceneID
{
    PersistentScene,
    LobbyScene,
    RaceScene,
}
