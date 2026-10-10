using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class BikerRigGroup : MonoBehaviour
{
    [Header("Rig1")]
    [SerializeField] private ChainIKConstraint rightHandChainIK;
    [SerializeField] private ChainIKConstraint leftHandChainIK;

    [Header("Rig2")]
    [SerializeField] private ChainIKConstraint headChainIK;

    [Header("Rig3")]
    [SerializeField] private TwoBoneIKConstraint rightFootTwoBoneIK;
    [SerializeField] private TwoBoneIKConstraint leftFootTwoBoneIK;

    public void SetRigsData(BikerInformation bikerInformation)
    {
        var rightHandData = rightHandChainIK.data;
        rightHandData.target = bikerInformation.TipRightHand;
        rightHandData.root = bikerInformation.RootRightHand;

        var leftHandData = leftHandChainIK.data;
        leftHandData.target = bikerInformation.TipLeftHand;
        leftHandData.root = bikerInformation.RootLeftHand;

        var headData = headChainIK.data;
        headData.target = bikerInformation.TipHead;

        var rightFootData = rightFootTwoBoneIK.data;
        rightFootData.target = bikerInformation.TipRightFoot;
        rightFootData.mid = bikerInformation.MidRightFoot;
        rightFootData.root = bikerInformation.RootRightFoot;

        var leftFootData = leftFootTwoBoneIK.data;
        leftFootData.target = bikerInformation.TipLeftFoot;
        leftFootData.mid = bikerInformation.MidLeftFoot;
        leftFootData.root = bikerInformation.RootLeftFoot;
    }
}
