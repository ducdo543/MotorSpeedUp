using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;
using Dreamteck.Splines;
using System;

#if UNITY_EDITOR
namespace Map
{
    public class MapCreator : MonoBehaviour
    {
        [Header("Spline Properties")]
        [SerializeField] private MapSplineProperties mapSplineProperties;

        [Header("References")]
        [SerializeField] private SplineComputer splineComputer;

        [Header("Number of Map Parts")]
        [SerializeField] private int numberOfMapParts;
        private int lastUsedMapPartIndex = 0;
        [SerializeField] private MapPart mapPartPrefab;
        [SerializeField] private Transform mapPartsParent;

        [Header("MapInfo")]
        public string baseFolderPath;


        private void OnValidate()
        {

            // if the number of map parts is changed in the inspector, we need to create or remove map parts accordingly
            if (lastUsedMapPartIndex != numberOfMapParts)
            {
                lastUsedMapPartIndex = numberOfMapParts;
                CreateGameObjectMapParts();
            }
        }

        // create map parts and assign the same spline computer to them
        private void CreateGameObjectMapParts()
        {
            if (mapPartsParent.childCount < numberOfMapParts)
            {
                for (int i = mapPartsParent.childCount; i < numberOfMapParts; i++)
                {
                    MapPart newMapPart = Instantiate(mapPartPrefab, mapPartsParent);
                    newMapPart.AssignSplineComputer(splineComputer);
                }
            }
            else if (mapPartsParent.childCount > numberOfMapParts)
            {
                numberOfMapParts = mapPartsParent.childCount;
            }
        }


        // create spline points 
        public void CreateSplinePoints()
        {
            var points = mapSplineProperties.GenerateRandomPoints();

            SplinePoint[] splinePoints = new SplinePoint[points.Count];

            for (int i = 0; i < points.Count; i++)
            {
                splinePoints[i] = new SplinePoint(points[i]);
                splinePoints[i].size = 1f;
                splinePoints[i].color = Color.white;
                splinePoints[i].normal = Vector3.up;
            }

            splineComputer.SetPoints(splinePoints);
            splineComputer.RebuildImmediate();


        }

        public void SaveMeshAndMapPrefab(int mapID, string prefabFolderPath)
        {
            // save meshes
            for (int i = 0; i < mapPartsParent.childCount; i++)
            {
                MapPart mapPart = mapPartsParent.GetChild(i).GetComponent<MapPart>();
                if (mapPart != null)
                {
                    mapPart.BakeAndSaveMeshes(baseFolderPath, mapID, i);
                }
            }

            // save map prefab
            GameObject mapPrefab = CreatingMapPrefab(mapID);

            string prefabPath = $"{prefabFolderPath}/Map_{mapID.ToString("D2")}.prefab";

            UnityEditor.PrefabUtility.SaveAsPrefabAsset(
                mapPrefab,
                prefabPath
            );

            // after saving the prefab, we can destroy the generated map prefab in the scene
            DestroyImmediate(mapPrefab);

            // unbake the meshes to free up memory
            for (int i = 0; i < mapPartsParent.childCount; i++)
            {
                MapPart mapPart = mapPartsParent.GetChild(i).GetComponent<MapPart>();
                if (mapPart != null)
                {
                    mapPart.UnbakeSplineMeshes();
                }
            }


        }

        private GameObject CreatingMapPrefab(int mapID)
        {
            GameObject mapParent = new GameObject($"Map_{mapID.ToString("D2")}");

            // creating map parts gameObjects without spline mesh components
            for (int i=0; i < mapPartsParent.childCount; i++)
            {
                GameObject mapPartGenerated = new GameObject($"MapPart_{i.ToString("D2")}");
                mapPartGenerated.AddComponent<MapPart>();
                mapPartGenerated.transform.SetParent(mapParent.transform);


                GameObject roadMesh = new GameObject($"RoadMesh");
                roadMesh.transform.SetParent(mapPartGenerated.transform);

                Mesh roadMeshAsset = mapPartsParent.GetChild(i).GetComponent<MapPart>().LoadRoadMeshAsset();
                roadMesh.AddComponent<MeshFilter>();
                roadMesh.GetComponent<MeshFilter>().sharedMesh = roadMeshAsset;
                roadMesh.AddComponent<MeshRenderer>();
                roadMesh.GetComponent<MeshRenderer>().sharedMaterial = mapPartsParent.GetChild(i).GetComponent<MapPart>().GetRoadMaterial();
                roadMesh.AddComponent<MeshCollider>();
                roadMesh.GetComponent<MeshCollider>().sharedMesh = roadMeshAsset;



                GameObject wallMesh = new GameObject($"WallMesh");
                wallMesh.transform.SetParent(mapPartGenerated.transform);

                Mesh wallMeshAsset = mapPartsParent.GetChild(i).GetComponent<MapPart>().LoadWallMeshAsset();
                wallMesh.AddComponent<MeshFilter>();
                wallMesh.GetComponent<MeshFilter>().sharedMesh = wallMeshAsset;
                wallMesh.AddComponent<MeshRenderer>();
                wallMesh.GetComponent<MeshRenderer>().sharedMaterial = mapPartsParent.GetChild(i).GetComponent<MapPart>().GetWallMaterial();
                wallMesh.AddComponent<MeshCollider>();
                wallMesh.GetComponent<MeshCollider>().sharedMesh = wallMeshAsset;

            }

            return mapParent;
        }    

        [Serializable]
        private struct MapSplineProperties
        {
            public float mapLengthZ;
            public float mapWidthX;
            public float mapHeightY;

            public float startCurvePointZ; // we need the first roadPart to be straight 

            public float minDistanceZBetweenPoints;
            public float maxDistanceZBetweenPoints;

            // create random points 
            public List<Vector3> GenerateRandomPoints()
            {
                List<Vector3> points = new List<Vector3>();

                Vector3 startPoint = new Vector3(0f, 0f, 0f);
                Vector3 startCurvePoint = new Vector3(0f, 0f, startCurvePointZ);
                Vector3 endPoint = new Vector3(0f, 0f, mapLengthZ);

                points.Add(startPoint);
                points.Add(startCurvePoint);

                float currentZ = startCurvePointZ;
                while (true)
                {
                    float randomDistanceZ = Random.Range(minDistanceZBetweenPoints, maxDistanceZBetweenPoints);
                    currentZ += randomDistanceZ;

                    if (currentZ >= mapLengthZ - minDistanceZBetweenPoints)
                    {
                        break;
                    }

                    float randomX = Random.Range(-mapWidthX / 2f, mapWidthX / 2f);
                    float randomY = Random.Range(-mapHeightY / 2f, mapHeightY / 2f);
                    points.Add(new Vector3(randomX, randomY, currentZ));

                }

                points.Add(endPoint);

                return points;
            }
        }
    }
}
#endif