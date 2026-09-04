using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class HazardController : MonoBehaviour
{
    [SerializeField] private bool canDead = false;
    public bool CanDead => canDead;
    [SerializeField] private bool isMotion = false;

    [Header("Hazard Properties")]
    private float delay = 0f;
    [SerializeField] private Transform theHolder; // the gameObject itself rotating, so we lose the original axes, use this instead
    [SerializeField] private HazardProperties hazardProperties;

    private void Start()
    {
        theHolder.localPosition = theHolder.localPosition + hazardProperties.offsetFromOriginPosition;
        theHolder.localRotation = theHolder.localRotation * Quaternion.Euler(hazardProperties.offsetFromOriginAngle);

        // delay is random between 0 and 1 second to avoid all hazards moving at the same time
        delay = UnityEngine.Random.Range(0f, 1f);

        //Invoke(nameof(MoveAndRotate), delay);
        MoveAndRotate();
    }

    private void MoveAndRotate()
    {
        StartCoroutine(Move());
        StartCoroutine(Rotate());

    }

    #region Move
    IEnumerator Move()
    {
        Vector3 startPosition = theHolder.localPosition;
        Vector3 endPosition = startPosition + hazardProperties.moveDistance;
        while (isMotion && hazardProperties.moveDistance != Vector3.zero)
        {
            switch (hazardProperties.moveType)
            {
                case LoopType.None:
                    yield return MoveTo(endPosition);
                    yield break; // exit the while loop
                case LoopType.Loop:
                    yield return MoveTo(endPosition);
                    yield return MoveTo(startPosition);
                    break; // break the switch, not the while loop
                case LoopType.Yoyo:
                    yield return MoveTo(endPosition);
                    theHolder.position = startPosition; // reset position to start
                    break; 
            }
        }
    }    

    IEnumerator MoveTo(Vector3 targetPosition)
    {
        float elapsedTime = 0f;
        Vector3 startPosition = theHolder.localPosition;
        while (elapsedTime < hazardProperties.moveDuration)
        {
            float t = elapsedTime / hazardProperties.moveDuration;
            float easedT = DOVirtual.EasedValue(0f, 1f, t, hazardProperties.moveEase);
            theHolder.localPosition = Vector3.Lerp(startPosition, targetPosition, easedT);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        theHolder.localPosition = targetPosition;
    }

    #endregion

    #region Rotate

    IEnumerator Rotate()
    {
        // we work with angle instead of quaternion to control the direction of rotation
        // cause slerp of quaternion guess the shortest path
        // but in this project, no need to
        Quaternion startRotation = theHolder.localRotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(hazardProperties.rotateAngle);
        while (isMotion && hazardProperties.rotateAngle != Vector3.zero)
        {
            switch (hazardProperties.rotateType)
            {
                case LoopType.None:
                    yield return RotateTo(endRotation);
                    yield break; // exit the while loop
                case LoopType.Loop:
                    yield return RotateTo(endRotation);
                    yield return RotateTo(startRotation);
                    break; // break the switch, not the while loop
                case LoopType.Yoyo:
                    yield return RotateTo(endRotation);
                    theHolder.rotation = startRotation; // reset rotation to start
                    break;
            }
        }
    }

    IEnumerator RotateTo(Quaternion targetRotation)
    {
        float elapsedTime = 0f;
        Quaternion startRotation = theHolder.localRotation;
        while (elapsedTime < hazardProperties.rotateDuration)
        {
            float t = elapsedTime / hazardProperties.rotateDuration;
            float easedT = DOVirtual.EasedValue(0f, 1f, t, hazardProperties.rotateEase);
            // debug t and easedT
            //Debug.Log($"RotateTo: t={t}, easedT={easedT}");
            theHolder.localRotation = Quaternion.Lerp(startRotation, targetRotation, easedT);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        theHolder.localRotation = targetRotation;
    }

    #endregion

}

[Serializable]
public struct HazardProperties
{
    [Header("Move")]
    public LoopType moveType;
    public Vector3 offsetFromOriginPosition;
    public Vector3 moveDistance;
    public float moveDuration;
    public Ease moveEase;

    [Header("Rotate")]
    public LoopType rotateType;
    public Vector3 offsetFromOriginAngle;
    public Vector3 rotateAngle;
    public float rotateDuration;
    public Ease rotateEase;
}

public enum LoopType
{
    None, // none can be means linearly move from start to end and stop
    Yoyo,
    Loop
}
