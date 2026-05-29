#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TrackPointFollower))]
public class TrackPointFollowerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TrackPointFollower controller =
            (TrackPointFollower)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Change Transform Based On Track Point Index"))
        {
            controller.ChangeTransformBasedOnTrackPoint();
        }
    }
}

#endif
