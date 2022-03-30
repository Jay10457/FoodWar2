using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Cooker : MonoBehaviourPunCallbacks
{
    public bool isCooking;
    public bool isOccupy;
    bool lastOccupy;
    public bool inCookArea;
    public FoodTeam cookerTeam;
    bool isPotionNearBy;
    public GameObject openRemain;
    PhotonView PV;
    MeshRenderer meshRenderer;
    RecipeManager recipeManager;
    public Item[] ingredients;
    string cookerId;
    int PVVeiwId;
    string userId;

    private void Awake()
    {
        meshRenderer = this.GetComponent<MeshRenderer>();
        PV = this.gameObject.GetComponent<PhotonView>();
        openRemain.SetActive(false);
        recipeManager = RecipeManager.instance;
        
        isOccupy = false;
       
    }


    public void SendToServerCooker(string _userId)
    {
       
            photonView.RPC("SendRequest", RpcTarget.MasterClient, _userId);
        
    }




    [PunRPC]
    private void SendRequest(string _userId)
    {
        userId = _userId;
        Debug.LogError(_userId);
        
    }







}