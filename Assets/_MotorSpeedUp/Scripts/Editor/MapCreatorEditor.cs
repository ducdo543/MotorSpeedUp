using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Map
{
    [CustomEditor(typeof(MapCreator))]
    public class MapCreatorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            MapCreator mapCreator = (MapCreator)target;

            GUILayout.Space(10);

            if (GUILayout.Button("Create Spline Points"))
            {
                mapCreator.CreateSplinePoints();
            }
        }
    }
}
