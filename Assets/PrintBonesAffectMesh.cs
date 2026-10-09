using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintBonesAffectMesh : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SkinnedMeshRenderer smr = GetComponent<SkinnedMeshRenderer>();

        foreach (Transform bone in smr.bones)
        {
            Debug.Log(bone.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
