using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryWeapon : MonoBehaviour
{
    [SerializeField] GameObject Bullet = null;
    [SerializeField] Transform launchPoint;
    [SerializeField] float force = 10f;
    [SerializeField] float flySpeed = 1f;

    [SerializeField] Vector3 launchToPos;

    Vector3 randomPosOffset = Vector3.zero;
    [SerializeField] TrajectoryManager tm;
    bool launch;




    private void Start()
    {
        tm = GetComponent<TrajectoryManager>();

        randomPosOffset = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), Random.Range(-5, 5));


    }
    private void Update()
    {
        ShootInput();
    }


    private void ShootInput()
    {

        AimState state = GetAimState();
        if (!CookUI.instance._isCookerOpen)
        {
            if (state == AimState.Start)
            {
                launchToPos = CrossHair.instance.transform.position;
                tm.CheckVector(launchToPos);
            }
            if (state == AimState.Move)
            {
                launchToPos = CrossHair.instance.transform.position;
                tm.CheckVector(launchToPos);
            }
            if (state == AimState.Ended)
            {
                launchToPos = CrossHair.instance.transform.position;
                tm.CheckVector(launchToPos);
                tm.line.positionCount = 0;
            }
            if (Input.GetMouseButtonDown(0) && state == AimState.Move)
            {

                tm.ShootObj(Bullet, launchToPos);
                HotBar.instance.WeaponUse();



            }
            else if (Input.GetMouseButtonDown(0) && state == AimState.None)
            {

                tm.ShootObj(Bullet, launchToPos + randomPosOffset);
                HotBar.instance.WeaponUse();

            }
        }

    }


    private AimState GetAimState()
    {

        if (Input.GetMouseButtonDown(1)) { return AimState.Start; }
        if (Input.GetMouseButton(1)) { return AimState.Move; }
        if (Input.GetMouseButtonUp(1)) { return AimState.Ended; }
        else
        {
            return AimState.None;
        }




    }

    private void FireBullet()
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
