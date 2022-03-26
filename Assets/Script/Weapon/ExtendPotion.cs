using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ExtendPotion : MonoBehaviourPunCallbacks
{
    bool isUseAble;
    GameObject currentTarget;
    [SerializeField] ParticleSystem potionFX;
    [SerializeField] PhotonView PV;




    public override void OnEnable()
    {
        base.OnEnable();
        currentTarget = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (PV.IsMine)
        {
            if (other.tag == "Pot")
            {
                isUseAble = true;
                currentTarget = other.gameObject;
               
            }
        }
        
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (PV.IsMine)
        {
            if (other.tag == "Pot")
            {
                isUseAble = false;
            
                currentTarget = null;
            }
        }
       
    }

    private void Update()
    {
        UsePotion();
    }

    private void UsePotion()
    {
        if (Input.GetMouseButtonDown(0) && isUseAble && !CookUI.instance.gameObject.activeSelf && currentTarget != null)
        {

            if (currentTarget.gameObject.GetComponent<Cooker>())
            {
                if (PV.IsMine)
                {
                    photonView.RPC("PotionFX", RpcTarget.All, currentTarget.transform.position);
                }
                
                HotBar.instance.WeaponUse();
                currentTarget.GetComponent<MeshRenderer>().material.color = Color.white;

            }
        }
    }
    [PunRPC]
    public void PotionFX(Vector3 pos)
    {
        Instantiate(potionFX, pos, Quaternion.Euler(-90, 0, 0));
    }

}
