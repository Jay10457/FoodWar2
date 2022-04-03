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
    
    public FoodTeam cookerTeam;
    public List<Item> materialsInCooker;
    public GameObject openRemain;
    PhotonView PV;
    MeshRenderer meshRenderer;
    RecipeManager recipeManager;
   
    string cookerId;
    int PVVeiwId;
    string userId;
   
    public Action<string, int> PutMaterialRPC;
    public Action<string, int> RemoveMaterialRPC;
    struct ItemPacket
    {
        public int itemId;
        public int amount;
        public string userId;
    }
  

    private new void OnEnable()
    {
        //Cooker to Ui
        PutMaterialRPC = SendMaterialToServer;
        RemoveMaterialRPC = RemoveMaterialFromServer;
      
    }
    private new void OnDisable()
    {
        //disable cooker to Ui
        PutMaterialRPC -= SendMaterialToServer;
        RemoveMaterialRPC -= RemoveMaterialFromServer;
    }


    private void Awake()
    {
       
        meshRenderer = this.GetComponent<MeshRenderer>();
        PV = this.gameObject.GetComponent<PhotonView>();
        openRemain.SetActive(false);
        recipeManager = RecipeManager.instance;
        materialsInCooker = new List<Item>();
  
      

    }
    private void Start()
    {
        myPlayerReference = FindObjectOfType<RoomManager>().myPlayer;

    }
   /// <summary>
   /// Send add material request to server
   /// </summary>
   /// <param name="itemPacket"></param>
   /// <param name="slotIndex"></param>
    public void SendMaterialToServer(string itemPacket, int slotIndex)
    {
        //Debug.LogError(string.Format("SendMaterial and item is {0}", itemPacket));
        photonView.RPC("SendAddMaterialRequest", RpcTarget.MasterClient, itemPacket, slotIndex);
    }
    /// <summary>
    /// Send remove material request to server
    /// </summary>
    /// <param name="itemPacket"></param>
    /// <param name="slotIndex"></param>
    public void RemoveMaterialFromServer(string itemPacket, int slotIndex)
    {

    }
    /// <summary>
    /// Client Sender: Client send request to server Who open the cooker
    /// </summary>
    /// <param name="_userId"></param>
    public void SendOpenRequestServerCooker(string _userId)
    {

        photonView.RPC("SendOpenRequest", RpcTarget.MasterClient, _userId);

    }

    /// <summary>
    /// Client Sender: Client send request to server Who close the cooker
    /// </summary>
    /// <param name="_userId"></param>
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
  

    /// <summary>
    /// Client Sender: Client send request to server Who add material
    /// </summary>
    /// <param name="_itemPacket"></param>
    /// <param name="_slotIndex"></param>
    [PunRPC]
    public void SendAddMaterialRequest(string _itemPacket, int _slotIndex)
    {
        ItemPacket itemPacket = JsonUtility.FromJson<ItemPacket>(_itemPacket);
        
        Item sendItem = ItemManager.instance.GetMaterialById(itemPacket.itemId);
        sendItem.slotIndex = _slotIndex;
        if (!materialsInCooker.Contains(sendItem))
        {
           

          

            Debug.LogError("add");
            materialsInCooker.Add(sendItem);
            photonView.RPC("ConfirmAddMaterialRequest", RpcTarget.All, _itemPacket, _slotIndex);
            Debug.LogError(string.Format("Material is {0}, amount is {1}, userId : {2}, from SlotIndex : {3}", sendItem.name, itemPacket.amount, itemPacket.userId, sendItem.slotIndex));
        }
        
       
    }

    
    /// <summary>
    /// Client reciver: Server confirm Client add material
    /// </summary>
    /// <param name="_itemPacket"></param>
    /// <param name="_slotIndex"></param>
    [PunRPC] 
    public void ConfirmAddMaterialRequest(string _itemPacket, int _slotIndex)
    {
        ItemPacket itemPacket = JsonUtility.FromJson<ItemPacket>(_itemPacket);
        Item reciveItem = ItemManager.instance.GetMaterialById(itemPacket.itemId);
        if (myPlayerReference.userId == itemPacket.userId || CookUI.instance.gameObject.activeSelf)
        {
            InventoryManager.instance.AddItemToInventoryInCurrentSlot(reciveItem, itemPacket.amount, CookUI.instance._ingredientSlots[_slotIndex]);
        }
    }




}