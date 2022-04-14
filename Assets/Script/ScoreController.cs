using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

public class ScoreController : MonoBehaviourPunCallbacks
{
    PhotonView PV;
    ScoreBarAndTimer score;
    HotBar hotBar;
    Item dish;
    [SerializeField] AudioClip[] scoreFXs;
    [SerializeField] AudioSource SFXplayer;

    public int currentDishId;
    [SerializeField] ParticleSystem scoreFX;

    int r_Score;
    public int g_Score;
    bool canSend;

    


    private void Awake()
    {
        PV = this.gameObject.GetPhotonView();
        hotBar = this.gameObject.GetComponent<HotBar>();
 
        
    }
    private void Start()
    {
        
        currentDishId = -1;
        score = ScoreBarAndTimer.instance;


    }
    private void SendDish()
    {
        if (currentDishId > 9 && Input.GetMouseButton(0) && canSend)
        {

            EventManager.instance.AddScore(ScoreCheck(currentDishId));
            photonView.RPC("SendFX", RpcTarget.All);
            SFXplayer.PlayOneShot(scoreFXs[UnityEngine.Random.Range(0, scoreFXs.Length)]);
            hotBar.WeaponUse();

        }
    }
    private void Update()
    {

        SendDish();


    }
    private void DetectTarget()
    {
        Collider[] cols = Physics.OverlapSphere(this.gameObject.transform.position, 5, 1 << LayerMask.NameToLayer("Target"));
        if (cols.Length > 0)
        {
            for (int i = 0; i < cols.Length; i++)
            {

                canSend = true;

            }
        }
        else
        {
            canSend = false;
        }


    }
    private void FixedUpdate()
    {
        DetectTarget();
        
    }

    private int ScoreCheck(int dishId)
    {
        dish = ItemManager.instance.GetMaterialById(dishId);
        return dish.score;
    }

 

    [PunRPC]
    private void SendFX()
    {
        Instantiate(scoreFX, this.gameObject.transform.position + new Vector3(0, 4, 0), Quaternion.identity);
    }



}
