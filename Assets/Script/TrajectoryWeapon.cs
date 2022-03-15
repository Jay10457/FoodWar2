using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryWeapon : MonoBehaviour
{
    [SerializeField] GameObject Bullet = null;
    [SerializeField] Transform launchPoint;
    [SerializeField] float force = 100f;
    [SerializeField] float flySpeed = 1f;
    bool launch;

    TrajectoryManager tm;

    private void Start()
    {
        tm = gameObject.AddComponent<TrajectoryManager>();
        
        tm.reuseLine = true;//set this to true so the line renderer gets reused every frame on prediction
        tm.accuracy = 0.99f;
        tm.lineWidth = 0.03f;
        tm.iterationLimit = 600;
    }
    private void Update()
    {
        ShootInput();
    }

    private void ShootInput()
    {
        if (Input.GetMouseButton(1))
        {
            tm.drawDebugOnPrediction = true;
            //Debug.LogError(tm.drawDebugOnPrediction);
        }
        else
        {
            tm.drawDebugOnPrediction = false;
            //Debug.LogError(tm.drawDebugOnPrediction);
        }
        if (Input.GetMouseButtonDown(0))
            launch = true;
        if (launch)
        {
            launch = false;

        }
    }

    private void FireBullet()
    {

    }
}
