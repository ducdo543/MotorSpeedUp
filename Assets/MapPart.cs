using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
using System.Reflection;
using UnityEditor;
using System;


public class MapPart : SplineUser
{
#if UNITY_EDITOR
    [SerializeField] private SplineMesh roadSplineMesh;
    [SerializeField] private SplineMesh wallSplineMesh;
    private string roadMeshAfterBakePath;
    private string wallMeshAfterBakePath;

    public void AssignSplineComputer(SplineComputer splineComputer)
    {
        spline = splineComputer;

        roadSplineMesh.spline = splineComputer;
        wallSplineMesh.spline = splineComputer;
        
    }
    public override void Rebuild()
    {
        base.Rebuild();
        TransferValues(roadSplineMesh);
        TransferValues(wallSplineMesh);
    }  
    
    private void TransferValues(SplineMesh splineMesh)
    {
        var splineUserType = typeof(SplineUser);
        var fields = splineUserType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

        foreach (var field in fields)
        {
            if (field.GetCustomAttribute<SerializeField>() != null || field.IsPublic)
            {
                var value = field.GetValue(this);
                field.SetValue(splineMesh, value);
            }

        }

        splineMesh.RebuildImmediate();
    }

    public void BakeAndSaveMeshes(string baseFolderPath, int mapID, int meshID)
    {
        roadSplineMesh.Bake(true, false);
        wallSplineMesh.Bake(true, false);

        Mesh roadMesh = roadSplineMesh.GetComponent<MeshFilter>()?.sharedMesh;
        Mesh wallMesh = wallSplineMesh.GetComponent<MeshFilter>()?.sharedMesh;

        // check if the mesh folder exists, if not, create it
        string meshFolderPath = $"{baseFolderPath}/Map_{mapID.ToString("D2")}/Meshes";
        if (!AssetDatabase.IsValidFolder(meshFolderPath))
        {
            AssetDatabase.CreateFolder($"{baseFolderPath}/Map_{mapID.ToString("D2")}", $"Meshes");
        }

        roadMeshAfterBakePath = $"{meshFolderPath}/RoadMesh_{meshID.ToString("D2")}.asset";
        wallMeshAfterBakePath = $"{meshFolderPath}/WallMesh_{meshID.ToString("D2")}.asset";

        SaveMeshAsset(roadMesh, roadMeshAfterBakePath);
        SaveMeshAsset(wallMesh, wallMeshAfterBakePath);

    }    

    private void SaveMeshAsset(Mesh mesh, string meshAfterBakePath)
    {
        if (mesh == null)
        {
            Debug.LogWarning($"Mesh is null, cannot save to {meshAfterBakePath}");
            return;
        }

        Mesh existingMesh = AssetDatabase.LoadAssetAtPath<Mesh>(meshAfterBakePath);
        if (existingMesh != null)
        {
            AssetDatabase.DeleteAsset(meshAfterBakePath);
        }

        Mesh meshClone = UnityEngine.Object.Instantiate(mesh);
        AssetDatabase.CreateAsset(meshClone, meshAfterBakePath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

    }

    public Material GetRoadMaterial()
    {
        Material roadMaterial = roadSplineMesh.GetComponent<MeshRenderer>()?.sharedMaterial;
        if (roadMaterial == null)
        {
            Debug.LogWarning("Road material is not assigned.");
        }
        return roadMaterial;
    }

    public Material GetWallMaterial()
    {
        Material wallMaterial = wallSplineMesh.GetComponent<MeshRenderer>()?.sharedMaterial;
        if (wallMaterial == null)
        {
            Debug.LogWarning("Wall material is not assigned.");
        }
        return wallMaterial;
    }

    public Mesh LoadRoadMeshAsset()
    {
        if (string.IsNullOrEmpty(roadMeshAfterBakePath))
        {
            Debug.LogWarning("Road mesh path is not set.");
            return null;
        }
        Mesh roadMesh = AssetDatabase.LoadAssetAtPath<Mesh>(roadMeshAfterBakePath);
        if (roadMesh == null)
        {
            Debug.LogWarning($"Failed to load road mesh from {roadMeshAfterBakePath}");
        }
        return roadMesh;
    }    

    public Mesh LoadWallMeshAsset()
    {
        if (string.IsNullOrEmpty(wallMeshAfterBakePath))
        {
            Debug.LogWarning("Wall mesh path is not set.");
            return null;
        }
        Mesh wallMesh = AssetDatabase.LoadAssetAtPath<Mesh>(wallMeshAfterBakePath);
        if (wallMesh == null)
        {
            Debug.LogWarning($"Failed to load wall mesh from {wallMeshAfterBakePath}");
        }
        return wallMesh;
    }

    public void UnbakeSplineMeshes()
    {
        roadSplineMesh.Unbake();
        wallSplineMesh.Unbake();
    }

    public void SetClipRange(float start, float end)
    {
        roadSplineMesh.clipFrom = start;
        roadSplineMesh.clipTo = end;
        wallSplineMesh.clipFrom = start;
        wallSplineMesh.clipTo = end;
        roadSplineMesh.RebuildImmediate();
        wallSplineMesh.RebuildImmediate();
    }

#endif
}
