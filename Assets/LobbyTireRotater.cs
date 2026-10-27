using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyTireRotater : MonoBehaviour
{
    [SerializeField] private TireSpinner tireSpinner;

    [SerializeField] private LobbyMapController lobbyMapController;


    private float frontTireRadius;
    private float backTireRadius;

    public void Initialize()
    {
        tireSpinner = GetComponentInChildren<TireSpinner>();

        frontTireRadius = GetTireRadius(tireSpinner.FrontTire);
        backTireRadius = GetTireRadius(tireSpinner.BackTire);
    }

    public void UpdateTires()
    {
        float moveSpeed = lobbyMapController.MoveSpeed;

        float rotateFrontSpeed = moveSpeed/frontTireRadius;
        float rotateBackSpeed = moveSpeed/backTireRadius;

        // rotate angle around Vector3.right axis
        float rotateFrontAngle = rotateFrontSpeed * Time.deltaTime * Mathf.Rad2Deg;
        float rotateBackAngle = rotateBackSpeed * Time.deltaTime * Mathf.Rad2Deg;

        Quaternion frontTargetRotation = tireSpinner.FrontTire.rotation * Quaternion.AngleAxis(rotateFrontAngle, Vector3.right);
        Quaternion backTargetRotation = tireSpinner.BackTire.rotation * Quaternion.AngleAxis(rotateBackAngle, Vector3.right);

        tireSpinner.RotateTires(frontTargetRotation, backTargetRotation);
    }

    private float GetTireRadius(Transform tire)
    {
        Renderer[] renderers = tire.GetComponentsInChildren<Renderer>();

        Bounds bounds = renderers[0].bounds;

        foreach (Renderer renderer in renderers)
        {
            bounds.Encapsulate(renderer.bounds);
        }

        float result = Mathf.Max(bounds.extents.y, bounds.extents.z);
        Debug.Log($"Result = {result}");
        // debug bounds size
        Debug.Log($"sizeY = {bounds.size.y}");
        return result;
    }    
}
