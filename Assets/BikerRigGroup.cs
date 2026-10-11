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

    [Header("RigBuilder")]
    [SerializeField] private RigBuilder rigBuilder;

    public void SetRigsData(BikerInformation bikerInformation)
    {
        var rightHandData = rightHandChainIK.data;
        rightHandData.tip = bikerInformation.TipRightHand;
        rightHandData.root = bikerInformation.RootRightHand;

        var leftHandData = leftHandChainIK.data;
        leftHandData.tip = bikerInformation.TipLeftHand;
        leftHandData.root = bikerInformation.RootLeftHand;

        var headData = headChainIK.data;
        headData.tip = bikerInformation.TipHead;
        headData.root = bikerInformation.RootHead;

        var rightFootData = rightFootTwoBoneIK.data;
        rightFootData.tip = bikerInformation.TipRightFoot;
        rightFootData.mid = bikerInformation.MidRightFoot;
        rightFootData.root = bikerInformation.RootRightFoot;

        var leftFootData = leftFootTwoBoneIK.data;
        leftFootData.tip = bikerInformation.TipLeftFoot;
        leftFootData.mid = bikerInformation.MidLeftFoot;
        leftFootData.root = bikerInformation.RootLeftFoot;

        rightHandChainIK.data = rightHandData;
        leftHandChainIK.data = leftHandData;
        headChainIK.data = headData;
        rightFootTwoBoneIK.data = rightFootData;
        leftFootTwoBoneIK.data = leftFootData;

        // Clear the previous data and Rebuild the rig after setting the data
        //rigBuilder.Clear();
        rigBuilder.Build();

    }
}
