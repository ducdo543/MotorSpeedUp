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

            if (GUILayout.Button("Create Spline Points"))
            {
                mapCreator.CreateSplinePoints();
            }

            // add save mesh and prefab button
            GUILayout.Space(10);
            EditorGUILayout.LabelField("Save Meshes", EditorStyles.boldLabel);
            mapID = EditorGUILayout.IntField("Map ID", mapID);
            if (GUILayout.Button("Save Meshes and Prefab"))
            {
                string prefabFolderPath = $"{mapCreator.baseFolderPath}/Map_{mapID}";
                if (!AssetDatabase.IsValidFolder(prefabFolderPath))
                {
                    AssetDatabase.CreateFolder(mapCreator.baseFolderPath, $"Map_{mapID}");
                }

                string prefabPath = $"{mapCreator.baseFolderPath}/Map_{mapID}/Map_{mapID}.prefab";
                if (AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath) != null)
                {
                    if (EditorUtility.DisplayDialog("Prefab already exists", $"A prefab with the name Map_{mapID} already exists. Do you want to create a new version of the prefab without overwriting the existing one?", "Yes", "No"))
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
