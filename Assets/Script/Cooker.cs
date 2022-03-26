using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Cooker : MonoBehaviourPunCallbacks
{
    public bool isCooking;
    public bool inCookArea;
    public FoodTeam cookerTeam;
    bool isPotionNearBy;
    PhotonView PV;
    MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = this.GetComponent<MeshRenderer>();
        PV = this.gameObject.GetComponent<PhotonView>();
    }
   
    private void OnTriggerStay(Collider other)
    {
        
        if (other.tag == "Potion")
        {
            isPotionNearBy = true;
        }
        else if (other.tag != "Potion")
        {
            isPotionNearBy = false;
            
        }
       
      
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Potion")
        {
            isPotionNearBy = false;
        }
    }
    private void Update()
    {
        ChangeColor();
        
        
    }

    private void ChangeColor()
    {
        if (isPotionNearBy)
        {
            meshRenderer.material.color = Color.red;
        }
        else
        {
            meshRenderer.material.color = Color.white;
        }
    }

}