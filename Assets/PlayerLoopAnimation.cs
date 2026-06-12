using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoopAnimation : MonoBehaviour
{
    [SerializeField] private AnimationClip clip;
    void Start()
    {
        Animator animator = GetComponent<Animator>();
        animator.Play(clip.name);
    }

    
}
