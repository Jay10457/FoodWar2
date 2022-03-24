using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHair : MonoBehaviour
{
    public static CrossHair instance;
    [SerializeField] Camera mainCam;
    Ray ray;
    RaycastHit hitInfo;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        mainCam = Camera.main;
    }
    private void Update()
    {
        if (mainCam != null)
        {
            ray.origin = mainCam.transform.position;
            ray.direction = mainCam.transform.forward;
            Physics.Raycast(ray, out hitInfo);
            if (hitInfo.collider != null && hitInfo.collider.tag != "ThrowObj" && hitInfo.collider.tag != "Obstacle"
                && hitInfo.collider.tag != "Pot" && hitInfo.collider.tag != "Player" && hitInfo.collider.tag != "Weapon")
            {
                transform.position = hitInfo.point;

                // Hit somthig here
                
            }
            else
            {
                transform.position = ray.GetPoint(75f);
            }
            
           
        }
       
    }
}
