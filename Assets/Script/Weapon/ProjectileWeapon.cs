using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class ProjectileWeapon : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject bullet;
   
    float shootForce = 100;
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
        if (photonView.IsMine)
        {
            ShootInput();
        }
        
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

        //GameObject currentBullet = Instantiate(bullet, firePoint.position, Quaternion.identity);
        //currentBullet.transform.forward = shootDir.normalized;
        // currentBullet.GetComponent<Rigidbody>().AddForce(shootDir.normalized * shootForce, ForceMode.Impulse);
        // 0~255
        firePoint.LookAt(targetPoint);
        photonView.RPC("RPCShoot", RpcTarget.All, firePoint.position, firePoint.rotation.eulerAngles);

        HotBar.instance.WeaponUse();
        Invoke("ResetShoot", 0.1f);
    }

    [PunRPC]
    public void RPCShoot(Vector3 startPos, Vector3 startDir, PhotonMessageInfo info)
    {
        GameObject temp = Instantiate(bullet, startPos, Quaternion.Euler(startDir));
        Bullet script = temp.GetComponent<Bullet>();
        // 求出訊息發送到接收的時間
        double lag = PhotonNetwork.Time - info.SentServerTime;
        script.speed = shootForce;
        script.LagMove(lag, this.transform.root.GetInstanceID());
    }

    void ResetShoot()
    {
        readyToShoot = true;
    }
}
