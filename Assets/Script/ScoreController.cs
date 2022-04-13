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
    ScoreManager scoreManager;

    public int currentDishId;
    [SerializeField] ParticleSystem scoreFX;

    int r_Score;
    public int g_Score;
    bool canSend;

    


    private void Awake()
    {
        PV = this.gameObject.GetPhotonView();
        hotBar = this.gameObject.GetComponent<HotBar>();
        scoreManager = FindObjectOfType<ScoreManager>();
        
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

            scoreManager.SendScoreRequest(ScoreCheck(currentDishId));
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

 





}
