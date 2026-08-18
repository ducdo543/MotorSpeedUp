using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikerSitAnimation : MonoBehaviour
{
    public AnimationClip clip;
    void Start()
    {
        clip.SampleAnimation(gameObject, 0f);
    }

    
}
