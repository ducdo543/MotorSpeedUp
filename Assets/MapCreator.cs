using Dreamteck.Splines;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

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

        [Header("Track Points")]
        [SerializeField] private int trackPointInterval;
        private List<TrackPoint> trackPoints;
        private HashSet<int> abyssTrackPointIndexes = new HashSet<int>();
        private HashSet<int> nearRespawnPointTrackPointIndexes = new HashSet<int>();

        [Header("Goal Point")]
        [SerializeField] private GameObject goalPointPrefab;


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

        // adjust map part clip range automatic according to the number of map parts
        public void AutoAdjustClipRange()
        {
            float totalLength = splineComputer.CalculateLength();
            float clipRange = totalLength / mapPartsParent.childCount;
            float clipRangePercentage = clipRange / totalLength;

            float startClip = 0f;
            float endClip = 0f;
            for (int i = 0; i < mapPartsParent.childCount; i++)
            {
                MapPart mapPart = mapPartsParent.GetChild(i).GetComponent<MapPart>();
                if (mapPart != null)
                {
                    endClip = startClip + clipRangePercentage;
                    mapPart.SetClipRange(startClip, endClip);
                    startClip = endClip;
                    
                }
            }
        }


        // create spline points 
        public void AutoCreateSplinePoints()
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
            AutoCreateTrackPoints();

            GameObject mapParent = new GameObject($"Map_{mapID.ToString("D2")}");
            MapController mapParentController = mapParent.AddComponent<MapController>();
            mapParentController.InitializeMap(mapID, trackPoints);
            List<TrackPoint> mapParentTrackPoints = mapParentController.TrackPoints;

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

            // creating goal gameObject
            GameObject goalPoint = Instantiate(goalPointPrefab, mapParent.transform);

            float length = splineComputer.CalculateLength();
            SplineSample goalPointSample = splineComputer.Evaluate((length - 100f) / length); // set the goal point at the end of the track, with an offset of 100 units
            
            goalPoint.transform.position = goalPointSample.position;
            goalPoint.transform.rotation = goalPointSample.rotation;


            // creating respawn points
            CreatingRespawnPoints(mapParent, mapParentTrackPoints);

            return mapParent;
        }    

        public void AutoCreateTrackPoints()
        {
            trackPoints = new List<TrackPoint>();
            float totalLength = splineComputer.CalculateLength();
            int numberOfTrackPoints = Mathf.FloorToInt(totalLength / trackPointInterval);

            int mapPartIndex = -1;
            double startClip = 0f;
            double endClip = 0f;

            for (int i = 0; i <= numberOfTrackPoints; i++)
            {
                SplineSample sample = splineComputer.Evaluate((float)i / numberOfTrackPoints);
                // see whether the track point is in the abyss, if the track point is out of clip range of all map parts, then it is in the abyss
                bool isInAbyss = false;


                while (mapPartIndex < mapPartsParent.childCount)
                {
                  
                    if (sample.percent >= startClip && sample.percent <= endClip)
                    {
                        break;
                    }
                    else if (sample.percent < startClip)
                    {
                        isInAbyss = true;
                        break;
                    }
                    else if (sample.percent > endClip)
                    {
                        mapPartIndex++;
                        MapPart mapPart = mapPartsParent.GetChild(mapPartIndex).GetComponent<MapPart>();
                        mapPart.GetClipRange(out startClip, out endClip);
                    }

                    
                }

                if (isInAbyss)
                {
                    abyssTrackPointIndexes.Add(i);
                }

                TrackPoint trackPoint = new TrackPoint(i, sample.position, (float)i / numberOfTrackPoints, sample.rotation);
                trackPoints.Add(trackPoint);

            }


        }

        private void CreatingRespawnPoints(GameObject mapParent, List<TrackPoint> mapParentTrackPoints)
        {
            MapController mapController = mapParent.GetComponent<MapController>();

            GameObject respawnPointsContainer = new GameObject("RespawnPoints");
            respawnPointsContainer.transform.SetParent(mapParent.transform);

            // respawn point interval is 9 track points, but if trackPoint is in the abyss, we need to check the next track point
            int index = 0;
            while (index < mapParentTrackPoints.Count)
            {
                
                if (!abyssTrackPointIndexes.Contains(index))
                {

                    GameObject respawnPoint = new GameObject($"RespawnPoint_{index.ToString("D2")}");
                    respawnPoint.transform.SetParent(respawnPointsContainer.transform);

                    RespawnPointController respawnPointController = respawnPoint.AddComponent<RespawnPointController>();
                    respawnPointController.trackPointIndexCorrespondingTo = index;
                    respawnPointController.ChangeTransformBasedOnTrackPoint(mapController);

                    // Around the respawn point, the index itself and the two indexes before and those of after are not allowed to generate hazard, we set a flag for them
                    for (int i = Mathf.Max(0, index - 2); i <= Mathf.Min(mapParentTrackPoints.Count - 1, index + 2); i++)
                    {
                        nearRespawnPointTrackPointIndexes.Add(i);
                    }

                    index += 9;
                }
                else
                {
                    
                        // if there is a respawn point before this abyss index in the range of 4 indexes, we need to delete it because the respawn point is too close to the abyss
                        // just need to see the latest respawn point: index - 9



                        //for (int i = Mathf.Max(0, index - 4); i < index; i++)
                        //{
                        //    // delete the respawn point gameobject
                        //    Transform respawnPointToDelete = respawnPointsContainer.transform.Find($"RespawnPoint_{i.ToString("D2")}");
                        //    if (respawnPointToDelete != null)
                        //    {
                        //        DestroyImmediate(respawnPointToDelete.gameObject);
                        //    }
                        //    nearRespawnPointTrackPointIndexes.Remove(i);
                        //}


                    index++;
                }
            }
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