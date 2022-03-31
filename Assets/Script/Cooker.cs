using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

public class Cooker : MonoBehaviourPunCallbacks
{
    [SerializeField] CookManager myPlayerReference = null;
    public bool isCooking;
    public bool isOccupy;
    public bool inCookArea;
    bool lastOccupy;
    bool isPotionNearBy;
    Item sendItem;
    public FoodTeam cookerTeam;
    
    public GameObject openRemain;
    PhotonView PV;
    MeshRenderer meshRenderer;
    RecipeManager recipeManager;
    public Item[] ingredients;
    string cookerId;
    int PVVeiwId;
    string userId;

    struct ItemPacket
    {
        public int itemId;
        public int amount;
    }

    private new void OnEnable()
    {
        //Cooker to Ui
        EventManager.instance.CookerToUI = SendMaterialToServer;
    }
    private new void OnDisable()
    {
        //disable cooker to Ui
        EventManager.instance.CookerToUI -= SendMaterialToServer;
    }


    private void Awake()
    {
       
        meshRenderer = this.GetComponent<MeshRenderer>();
        PV = this.gameObject.GetComponent<PhotonView>();
        openRemain.SetActive(false);
        recipeManager = RecipeManager.instance;
        ingredients = new Item[3];

    }
    private void Start()
    {
        myPlayerReference = FindObjectOfType<RoomManager>().myPlayer;

    }
   
    public void SendMaterialToServer(string itemPacket)
    {
        Debug.LogError(string.Format("SendMaterial and item is {0}", itemPacket));
        photonView.RPC("SendAddMaterialRequest", RpcTarget.MasterClient, itemPacket);
    }
    public void SendOpenRequestServerCooker(string _userId)
    {

        photonView.RPC("SendOpenRequest", RpcTarget.MasterClient, _userId);

    }
    public void SendCloseRequestServerCooker(string _userId)
    {
        photonView.RPC("SendCloseRequest", RpcTarget.MasterClient, _userId);
    }

    #region OpenUIRPC
    [PunRPC]
    private void SendCloseRequest(string _userId)
    {
        userId = _userId;
        photonView.RPC("RespondCloseRPCCooker", RpcTarget.All, _userId);
    }

    [PunRPC]
    private void SendOpenRequest(string _userId)
    {
        userId = _userId;
        //Debug.LogError(_userId);
        photonView.RPC("RespondOpenRPCCooker", RpcTarget.All, _userId);

    }
    
    [PunRPC]
    private void RespondOpenRPCCooker(string _userId)
    {
       // Debug.LogError(string.Format("your Id is {0}", _userId));
        if (myPlayerReference.userId == _userId)
        {
            
            CookUI.instance.gameObject.SetActive(true);
            CookUI.instance.SetCookerUIBillboard(this.transform.position);
            Cursor.lockState = CursorLockMode.None;
        }
       
    }
    [PunRPC]
    private void RespondCloseRPCCooker(string _userId)
    {
        //Debug.LogError(string.Format("your Id is {0}", _userId));
        if (myPlayerReference.userId == _userId)
        {
            
            CookUI.instance.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            //CookUI.instance.SetCookerUIBillboard(this.transform.position);
        }

    }
    #endregion

    [PunRPC]
    public void SendAddMaterialRequest(string _itemPacket)
    {
       
        ItemPacket itemPacket = JsonUtility.FromJson<ItemPacket>(_itemPacket);

        sendItem = ItemManager.instance.GetMaterialById(itemPacket.itemId);
        ingredients[0] = sendItem;
        Debug.LogError(string.Format("Material is {0}, amount is {1}", sendItem.name, itemPacket.amount));
    }




}