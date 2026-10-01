using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikerSitAnimation : MonoBehaviour
{
    public AnimationClip clip;
    void Start()
    {
        if (clip != null)
        {
            clip.SampleAnimation(gameObject, 0f);
        }
    }

    public void SetSitAnimation(AnimationClip newClip)
    {
        clip = newClip;
        clip.SampleAnimation(gameObject, 0f);
    }
}
