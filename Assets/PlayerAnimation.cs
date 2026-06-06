using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public AnimationClip clip;
    void Start()
    {
        clip.SampleAnimation(gameObject, 0f);
    }

    
}
