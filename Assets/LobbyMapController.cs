using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyMapController : MonoBehaviour
{
    [SerializeField] private List<Transform> roadParts;
    [SerializeField] private Transform resetPoint;
    [SerializeField] private float moveSpeed = 5f;

    public float MoveSpeed => moveSpeed;
    void Start()
    {
        int level = GameManager.Instance.DataManager.MotorSpeedUpData.Level;
        Initialize(level);
    }

    public void Initialize(int level)
    {
        Material firstRoadMaterial = DataReader.Instance.GetFirstMaterialOfMap(level);
        ChangeRoadsMaterial(firstRoadMaterial);
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
        //Debug.Log("Total Length of Road Parts: " + totalLength);

        return totalLength;
    }

    // changing material of road parts
    public void ChangeRoadsMaterial(Material newMaterial)
    {
        foreach (Transform part in roadParts)
        {
            Renderer renderer = part.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = newMaterial;
            }
        }
    }

}
