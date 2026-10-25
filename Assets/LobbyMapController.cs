using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyMapController : MonoBehaviour
{
    [SerializeField] private List<Transform> roadParts;
    [SerializeField] private Transform resetPoint;
    [SerializeField] private float moveSpeed = 5f;
    void Start()
    {
        
    }

    
    void Update()
    {
        MoveAndReset();
    }

    private void MoveAndReset()
    {
        foreach (Transform part in roadParts)
        {
            part.Translate(Vector3.back * moveSpeed * Time.deltaTime);

            if (part.position.z < resetPoint.position.z)
            {
                float totalLength = GetTotalLength();
                part.position += Vector3.forward * totalLength;
            }
        }
    }    

    private float GetTotalLength()
    {
        float totalLength = 0f;
        foreach (Transform part in roadParts)
        {
            totalLength += part.GetComponent<Renderer>().bounds.size.z;
        }
        Debug.Log("Total Length of Road Parts: " + totalLength);

        return totalLength;
    }

}
