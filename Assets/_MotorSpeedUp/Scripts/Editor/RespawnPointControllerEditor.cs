#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RespawnPointController))]
public class RespawnPointControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RespawnPointController controller =
            (RespawnPointController)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Change Transform Based On Track Point Index"))
        {
            controller.ChangeTransformBasedOnTrackPoint();
        }
    }
}

#endif
