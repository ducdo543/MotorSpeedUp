using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorController : MonoBehaviour
{
    [SerializeField] private Transform pointForBiker;
    [SerializeField] private BikerController bikerController;
    private Vector3 offSetLocal;
    void Start()
    {
        offSetLocal = pointForBiker.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 offSetWorld = transform.rotation * offSetLocal;
        bikerController.ChangeTransform(transform.position + offSetWorld, transform.rotation);
    }
}
