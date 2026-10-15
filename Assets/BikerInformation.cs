using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikerInformation : MonoBehaviour
{
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

}
