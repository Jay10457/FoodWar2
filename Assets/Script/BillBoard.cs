using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    Camera cam;
    private void Awake()
    {
        cam = Camera.main;

    }
    private void LateUpdate()
    {
        this.transform.LookAt(cam.transform);
        this.transform.rotation = Quaternion.LookRotation(cam.transform.forward);
    }
}
