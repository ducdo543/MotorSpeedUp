using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    private Scene currentScene;

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

    public void LoadSceneCustom(SceneID newSceneID)
    {
        string newSceneName = IsEqualSceneName(newSceneID);
        // get currentScene if it's not assigned
        if (!currentScene.IsValid())
        {
            currentScene = SceneManager.GetActiveScene();
        }

        // set active scene to PersistentScene temporarily to avoid issues when unloading current scene
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("PersistentScene"));

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
        };
        return loadOp;
    }

    private string IsEqualSceneName(SceneID sceneID)
    {
        switch (sceneID)
        {
            case SceneID.PersistentScene:
                return "PersistentScene";
            case SceneID.IdleScene:
                return "IdleScene";
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
    IdleScene,
    RaceScene,
}
