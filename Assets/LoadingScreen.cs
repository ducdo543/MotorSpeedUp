using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadFirstScene());
    }

    IEnumerator LoadFirstScene()
    {
        // Simulate loading time, and wait for SceneLoader.Instance to be created
        yield return new WaitForSecondsRealtime(3f);


        AsyncOperation op = SceneLoader.Instance.LoadFirstSceneCustom(SceneID.RaceScene);
    }
}
