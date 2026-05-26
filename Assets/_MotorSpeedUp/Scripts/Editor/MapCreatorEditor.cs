using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Map
{
    [CustomEditor(typeof(MapCreator))]
    public class MapCreatorEditor : Editor
    {
        int mapID;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            MapCreator mapCreator = (MapCreator)target;

            GUILayout.Space(10);

            if (GUILayout.Button("Auto Create Spline Points"))
            {
                mapCreator.AutoCreateSplinePoints();
            }

            // add button to adjust clip range of map parts
            GUILayout.Space(10);
            if (GUILayout.Button("Auto Adjust Clip Range"))
            {
                mapCreator.AutoAdjustClipRange();
            }

            // add button to auto create track points
            GUILayout.Space(10);
            if (GUILayout.Button("Auto Create Track Points"))
            {
                mapCreator.AutoCreateTrackPoints();
            }

            // add save mesh and prefab button
            GUILayout.Space(10);
            EditorGUILayout.LabelField("Save Meshes", EditorStyles.boldLabel);
            mapID = EditorGUILayout.IntField("Map ID", mapID);
            if (GUILayout.Button("Save Meshes and Prefab"))
            {
                string prefabFolderPath = $"{mapCreator.baseFolderPath}/Map_{mapID.ToString("D2")}";
                if (!AssetDatabase.IsValidFolder(prefabFolderPath))
                {
                    AssetDatabase.CreateFolder(mapCreator.baseFolderPath, $"Map_{mapID.ToString("D2")}");
                }

                string prefabPath = $"{mapCreator.baseFolderPath}/Map_{mapID.ToString("D2")}/Map_{mapID.ToString("D2")}.prefab";
                if (AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath) != null)
                {
                    if (EditorUtility.DisplayDialog("Prefab already exists", $"A prefab with the name Map_{mapID.ToString("D2")} already exists. Do you want to overwrite the existing one?", "Yes", "No"))
                    {
                        mapCreator.SaveMeshAndMapPrefab(mapID, prefabFolderPath);
                    }
                }
                else
                {
                    mapCreator.SaveMeshAndMapPrefab(mapID, prefabFolderPath);
                }
            }
        }
    }
}
