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
        if (cam != null)
        {

            gameObject.transform.LookAt(cam.transform);

            gameObject.transform.rotation = Quaternion.LookRotation(cam.transform.forward);
            
        }
        else
        {
            cam = Camera.main;
        }

    }
}
