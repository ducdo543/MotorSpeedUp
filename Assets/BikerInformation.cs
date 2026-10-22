using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikerInformation : MonoBehaviour
{
#if UNITY_EDITOR
    [Header("UnityEditorOnly")]
    [SerializeField] private Avatar humanoidAvatar;
#endif

    [Header("RightHand")]
    [SerializeField] private Transform rootRightHand;
    [SerializeField] private Transform tipRightHand;

    public Transform RootRightHand => rootRightHand;
    public Transform TipRightHand => tipRightHand;

    [Header("LeftHand")]
    [SerializeField] private Transform rootLeftHand;
    [SerializeField] private Transform tipLeftHand;
    public Transform RootLeftHand => rootLeftHand;
    public Transform TipLeftHand => tipLeftHand;

    [Header("Head")]
    [SerializeField] private Transform rootHead;
    [SerializeField] private Transform tipHead;
    public Transform RootHead => rootHead;
    public Transform TipHead => tipHead;

    [Header("Hips")]
    [SerializeField] private Transform constrainedHips;
    public Transform ConstrainedHips => constrainedHips;

    [Header("RightFoot")]
    [SerializeField] private Transform rootRightFoot;
    [SerializeField] private Transform midRightFoot;
    [SerializeField] private Transform tipRightFoot;

    public Transform RootRightFoot => rootRightFoot;
    public Transform MidRightFoot => midRightFoot;
    public Transform TipRightFoot => tipRightFoot;

    [Header("LeftFoot")]
    [SerializeField] private Transform rootLeftFoot;
    [SerializeField] private Transform midLeftFoot;
    [SerializeField] private Transform tipLeftFoot;
    public Transform RootLeftFoot => rootLeftFoot;
    public Transform MidLeftFoot => midLeftFoot;
    public Transform TipLeftFoot => tipLeftFoot;

#if UNITY_EDITOR
    public void AutomaticallyGetBones()
    {
        Animator animator = gameObject.AddComponent<Animator>();
        animator.avatar = humanoidAvatar;

        rootRightHand = animator.GetBoneTransform(HumanBodyBones.RightUpperArm);
        tipRightHand = animator.GetBoneTransform(HumanBodyBones.RightHand);

        rootLeftHand = animator.GetBoneTransform(HumanBodyBones.LeftUpperArm);
        tipLeftHand = animator.GetBoneTransform(HumanBodyBones.LeftHand);

        rootHead = animator.GetBoneTransform(HumanBodyBones.Spine);
        tipHead = animator.GetBoneTransform(HumanBodyBones.Head);

        constrainedHips = animator.GetBoneTransform(HumanBodyBones.Hips);

        rootRightFoot = animator.GetBoneTransform(HumanBodyBones.RightUpperLeg);
        midRightFoot = animator.GetBoneTransform(HumanBodyBones.RightLowerLeg);
        tipRightFoot = animator.GetBoneTransform(HumanBodyBones.RightFoot);

        rootLeftFoot = animator.GetBoneTransform(HumanBodyBones.LeftUpperLeg);
        midLeftFoot = animator.GetBoneTransform(HumanBodyBones.LeftLowerLeg);
        tipLeftFoot = animator.GetBoneTransform(HumanBodyBones.LeftFoot);

        // remove the animator component after getting the bones
        DestroyImmediate(animator, true);
    }
#endif

}
