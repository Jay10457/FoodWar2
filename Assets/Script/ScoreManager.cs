using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

public class ScoreManager : MonoBehaviourPunCallbacks
{
    PhotonView PV;
    ScoreBarAndTimer score;
    HotBar hotBar;
    Item dish;
  
    public int currentDishId;


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
        if (currentDishId > 9 && Input.GetMouseButton(0))
        {
            SendAddScoreRequest(ScoreCheck(currentDishId));
            hotBar.WeaponUse();
            
        }
    }
    private void Update()
    {
        SendDish();
    }

    private int ScoreCheck(int dishId)
    {
        dish = ItemManager.instance.GetMaterialById(dishId);
        return dish.score;
    }

    private void SendAddScoreRequest(int _score)
    {
        photonView.RPC("SendAddScoreRequestToServer", RpcTarget.MasterClient, _score);
    }

    [PunRPC]
    private void SendAddScoreRequestToServer(int _score)
    {
        Debug.LogError("you score is :" + _score);
    }
}
