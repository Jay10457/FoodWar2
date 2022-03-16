using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryWeapon : MonoBehaviour
{
    [SerializeField] GameObject Bullet = null;
    [SerializeField] Transform launchPoint;
    [SerializeField] float force = 10f;
    [SerializeField] float flySpeed = 1f;
    [SerializeField] CrossHair crossHair;
    [SerializeField] Camera cam;
    [SerializeField] Vector3 launchToPos;
    
    [SerializeField] TrajectoryManager tm;
    bool launch;

   

    private void Start()
    {
        tm = GetComponent<TrajectoryManager>();
        cam = Camera.main;
        crossHair = cam.GetComponentInChildren<CrossHair>();
       
       
    }
    private void Update()
    {
        ShootInput();
    }

    private void ShootInput()
    {
        AimState state = GetAimState();
        if (state == AimState.Start)
        {
            launchToPos = crossHair.transform.position;
            tm.CheckVector(launchToPos);
        }
        if (state == AimState.Move)
        {
            launchToPos = crossHair.transform.position;
            tm.CheckVector(launchToPos);
        }
        if (state == AimState.Ended)
        {
            launchToPos = crossHair.transform.position;
            tm.CheckVector(launchToPos);
            tm.line.positionCount = 0;
        }
       
    }
    private AimState GetAimState()
    {
        if (Input.GetMouseButtonDown(1)) { return AimState.Start; }
        if (Input.GetMouseButton(1)) { return AimState.Move; }
        if (Input.GetMouseButtonUp(1)) { return AimState.Ended; }

        return AimState.None;
    }

    private void FireBullet()
    {

    }
    private void LateUpdate()
    {
        
    }
    private enum AimState
    {
        Start = 0,
        Move = 1,
        Stay = 2,
        Ended = 3,

        None = 9
    }
}
