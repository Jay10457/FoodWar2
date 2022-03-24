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
    

    private void OnTriggerEnter(Collider other)
    {
        if (PV.IsMine)
        {
            if (other.tag == "Pot")
            {
                isUseAble = true;
                currentTarget = other.gameObject;
                currentTarget.GetComponent<MeshRenderer>().material.color = Color.red;
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
                currentTarget.GetComponent<MeshRenderer>().material.color = Color.white;
                currentTarget = null;
            }
        }
       
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && isUseAble && !CookUI.instance.gameObject.activeSelf)
        {
            
            if (currentTarget.gameObject.GetComponent<Cooker>())
            {
                Instantiate(potionFX, currentTarget.transform.position, Quaternion.Euler(-90, 0, 0));
                HotBar.instance.WeaponUse();
                currentTarget.GetComponent<MeshRenderer>().material.color = Color.white;
            }
        }
    }

}
