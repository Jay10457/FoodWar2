using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : MonoBehaviour
{
    [SerializeField] GameObject bullet;
   
    [SerializeField] float shootForce = 400;
    [SerializeField] float upwardForce;
    [SerializeField] float timeBetweenShooting;
    [SerializeField] float spread;
    [SerializeField] float timeBetweenShoot;
    [SerializeField] Transform firePoint;
    [SerializeField] Camera cam;
  


    [SerializeField] CrossHair crossHair;
    bool readyToShoot;

    private void Start()
    {
        cam = Camera.main;
        crossHair = cam.gameObject.GetComponentInChildren<CrossHair>();
        readyToShoot = true;
        
    }
    private void Update()
    {
        ShootInput();
    }

    void ShootInput()
    {
        if (Input.GetMouseButtonDown(0) && readyToShoot && !CookUI.instance._isCookerOpen)
        {
            //Debug.LogError("shoot");
            Shoot();
        }
    }

    void Shoot()
    {
        readyToShoot = false;
        Vector3 targetPoint = crossHair.gameObject.transform.position;

        Vector3 shootDir = targetPoint - firePoint.position;

        GameObject currentBullet = Instantiate(bullet, firePoint.position, Quaternion.identity);
        currentBullet.transform.forward = shootDir.normalized;

        currentBullet.GetComponent<Rigidbody>().AddForce(shootDir.normalized * shootForce, ForceMode.Impulse);
        HotBar.instance.WeaponUse();
        Invoke("ResetShoot", 0.1f);
    }

    void ResetShoot()
    {
        readyToShoot = true;
    }
}
