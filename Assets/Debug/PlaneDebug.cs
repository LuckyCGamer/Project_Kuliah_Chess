using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneDebug : MonoBehaviour
{
    void Start()
    {
        Camera cam = Camera.main;

        Vector3 planePosition = new Vector3(0, 0, 0);
        transform.rotation = Quaternion.FromToRotation(Vector3.up, -cam.transform.forward);
    }
}
