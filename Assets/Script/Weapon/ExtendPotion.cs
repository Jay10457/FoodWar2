using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

public class ExtendPotion : MonoBehaviourPunCallbacks
{
   
    Cooker currentTarget;
    [SerializeField] ParticleSystem potionFX;
    [SerializeField] PhotonView PV;
    [SerializeField] CookController myPlayer;

    

    private void Awake()
    {
        myPlayer = this.gameObject.transform.root.GetComponent<CookController>();
    }

    private void Update()
    {
        if (myPlayer.canUsePotion)
        {
            UsePotion();
        }
        if (myPlayer.currentCooker != null)
        {
            currentTarget = myPlayer.currentCooker;
        }
        
    }

    private void UsePotion()
    {
        if (Input.GetMouseButtonDown(0)&& !CookUI.instance.gameObject.activeSelf && currentTarget != null)
        {

            if (currentTarget != null)
            {
                if (PV.IsMine)
                {
                    currentTarget.AddCookerTime(10f);
                    photonView.RPC("PotionFX", RpcTarget.All, currentTarget.gameObject.transform.position);
                }
                
                HotBar.instance.WeaponUse();
                

            }
        }
    }
    [PunRPC]
    public void PotionFX(Vector3 pos)
    {
        Instantiate(potionFX, pos, Quaternion.Euler(-90, 0, 0));
    }

}
