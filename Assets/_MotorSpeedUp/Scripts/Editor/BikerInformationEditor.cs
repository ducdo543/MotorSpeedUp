using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BikerInformation))]
public class BikerInformationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        BikerInformation bikerInformation = (BikerInformation)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Automatically Get Bones From Avatar"))
        {
            bikerInformation.AutomaticallyGetBones();
        }
    }
}
