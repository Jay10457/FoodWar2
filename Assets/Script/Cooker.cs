using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.UI;

[Serializable]
public class MaterialListSerializable<T>
{
    public List<T> materialsStore;
}


public partial class Cooker : MonoBehaviourPunCallbacks
{
    [SerializeField] CookController myPlayerReference = null;
    [SerializeField] MaterialListSerializable<ItemStore> materialsInCooker;
    [SerializeField] ItemStore itemStore = new ItemStore();
    public SpriteRenderer resultIconDisplay = null;
    public Canvas cookingProgress = null;
    [SerializeField] Image cookingProgressBar = null;
    [SerializeField] AudioSource SFXPlayer;
    [SerializeField] AudioClip dingFX;
    [Range(0, 1)]
    [SerializeField] float pitch;


    public bool isCooking;





    public FoodTeam cookerTeam;


    public GameObject openRemain;
    PhotonView PV;
    MeshRenderer meshRenderer;
    [SerializeField] ParticleSystem PotFX;
   


    string cookerId;
    public int PVVeiwId;
    string userId;
    public float cookStartTime;
    public Action<string, int> PutMaterialRPC;
    public Action<string, int> RemoveMaterialRPC;
    public Action<string> RefreshUIFromServerRPC;
    public Action<int> StartCookRPC;
    public Action<int> PickUpDishRPC;
    struct ItemPacket
    {
        public int itemId;
        public int amount;
        public string userId;
    }

    [Serializable]
    struct ItemStore
    {
        public int itemId;
        public int amount;
        public int slotIndex;
    }


    private new void OnEnable()
    {
        //Cooker to Ui
        RefreshUIFromServerRPC = SendRefreshCookerUI;
        PutMaterialRPC = SendMaterialToServer;
        RemoveMaterialRPC = RemoveMaterialFromServer;
        StartCookRPC = StartCookRequestToServer;
        CheckIngredient = CheckIngredientsMethod;


    }
    private new void OnDisable()
    {
        //disable cooker to Ui
        PutMaterialRPC -= SendMaterialToServer;
        RemoveMaterialRPC -= RemoveMaterialFromServer;
        RefreshUIFromServerRPC -= SendRefreshCookerUI;
        StartCookRPC -= StartCookRequestToServer;
        CheckIngredient -= CheckIngredientsMethod;


    }


    private void Awake()
    {
        cookingProgressBar.fillAmount = 0;
        cookingProgress.gameObject.SetActive(false);

        meshRenderer = this.GetComponent<MeshRenderer>();
        PV = this.gameObject.GetComponent<PhotonView>();
        PVVeiwId = PV.ViewID;
        openRemain.SetActive(false);
        ingredients = new List<Item>();
        recipes = RecipeManager.instance.recipes;
        materialsInCooker = new MaterialListSerializable<ItemStore>();
        if (cookerTeam == FoodTeam.GOOD)
        {
            pitch = 1;
        }
        else
        {
            pitch = 0.8f;
        }




    }
    private void Start()
    {
        resultIconDisplay.gameObject.SetActive(false);
        myPlayerReference = FindObjectOfType<RoomManager>().myPlayer;
        PotFX.Stop();

    }
    public void SendRefreshCookerUI(string _userId)
    {
        photonView.RPC("SendRefreshRequest", RpcTarget.MasterClient, _userId);
        //Debug.LogError(string.Format("ID: {0} send refrash request", _userId));
    }

    public void StartCookRequestToServer(int _cookerId)
    {
        photonView.RPC("StartCookRequest", RpcTarget.MasterClient, _cookerId);
    }

    public void PickUPDishRequestToServer(int _cookerId, string _userId)
    {
        photonView.RPC("PickUpRequest", RpcTarget.MasterClient, _cookerId, _userId);
    }

    /// <summary>
    /// Send add material request to server
    /// </summary>
    /// <param name="itemPacket"></param>
    /// <param name="slotIndex"></param>
    public void SendMaterialToServer(string itemPacket, int slotIndex)
    {
        //Debug.LogError(string.Format("SendMaterial and item is {0}", itemPacket));
        //Debug.LogError("AddRequest");
        photonView.RPC("SendAddMaterialRequest", RpcTarget.MasterClient, itemPacket, slotIndex);

    }
    


    public void AddCookerTime(float addTime)
    {
        photonView.RPC("AddTimeRpc", RpcTarget.All, addTime);
    }

    [PunRPC]
    private void AddTimeRpc(float addTime)
    {
        cookStartTime = cookStartTime + addTime;
    }
    public IEnumerator CookingProgress(float time, string recepeName)
    {

        
        cookStartTime = Time.time;
        while (Time.time < cookStartTime + time)
        {
            cookingProgressBar.fillAmount = Mathf.Abs((((cookStartTime + time) - Time.time) / time) - 1);
            

            yield return null;






        }
        
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("CookDone", RpcTarget.All, recepeName);
        }
       

    }
    /// <summary>
    /// Send remove material request to server
    /// </summary>
    /// <param name="itemPacket"></param>
    /// <param name="slotIndex"></param>
    public void RemoveMaterialFromServer(string itemPacket, int slotIndex)
    {
        photonView.RPC("SendRemoveMaterialRequest", RpcTarget.MasterClient, itemPacket, slotIndex);
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

    #region Add and Remove Material
    [PunRPC]
    public void SendRemoveMaterialRequest(string _itemPacket, int _slotIndex)
    {
        ItemPacket itemPacket = JsonUtility.FromJson<ItemPacket>(_itemPacket);

        itemStore.itemId = itemPacket.itemId;
        itemStore.amount = itemPacket.amount;
        itemStore.slotIndex = _slotIndex;
        Item storeItem;
        storeItem = ItemManager.instance.GetMaterialById(itemStore.itemId);
        // Debug.LogError(string.Format("Want remove material is {0}", sendItem.name));
        if (materialsInCooker.materialsStore.Contains(itemStore))
        {
            materialsInCooker.materialsStore.Remove(itemStore);
            photonView.RPC("ConfirmRemoveMaterialRequest", RpcTarget.All, _itemPacket, _slotIndex, PVVeiwId);

        }
        if (ingredients.Contains(storeItem))
        {
            ingredients.Remove(storeItem);
        }
    }
    /// <summary>
    /// Client Sender: Client send request to server Who add material
    /// </summary>
    /// <param name="_itemPacket"></param>
    /// <param name="_slotIndex"></param>
    [PunRPC]
    public void SendAddMaterialRequest(string _itemPacket, int _slotIndex)
    {
        ItemPacket itemPacket = JsonUtility.FromJson<ItemPacket>(_itemPacket);

        Item storeItem;
        itemStore.itemId = itemPacket.itemId;
        itemStore.amount = itemPacket.amount;
        itemStore.slotIndex = _slotIndex;
        storeItem = ItemManager.instance.GetMaterialById(itemStore.itemId);

        if (!materialsInCooker.materialsStore.Exists(t => t.itemId == itemPacket.itemId))
        {




            // Debug.LogError("add");
            materialsInCooker.materialsStore.Add(itemStore);
            photonView.RPC("ConfirmAddMaterialRequest", RpcTarget.All, _itemPacket, _slotIndex, PVVeiwId);

            //Debug.LogError(string.Format("Material is {0}, amount is {1}, userId : {2}, from SlotIndex : {3}", sendItem.name, itemPacket.amount, itemPacket.userId, sendItem.slotIndex));
        }
        if (!ingredients.Contains(storeItem))
        {
            ingredients.Add(storeItem);
        }


    }


    /// <summary>
    /// Client reciver: Server confirm Client add material
    /// </summary>
    /// <param name="_itemPacket"></param>
    /// <param name="_slotIndex"></param>
    [PunRPC]
    public void ConfirmAddMaterialRequest(string _itemPacket, int _slotIndex, int _viewId)
    {
        ItemPacket itemPacket = JsonUtility.FromJson<ItemPacket>(_itemPacket);
        Item reciveItem = ItemManager.instance.GetMaterialById(itemPacket.itemId);
        //Debug.LogError(itemPacket.userId);
        if (myPlayerReference.userId == itemPacket.userId)
        {

            MaterialSlot.instance.reduceMaterialAmount();
        }
        if (CookUI.instance.gameObject.activeSelf && _viewId == myPlayerReference.currentCooker.PVVeiwId)
        {
            InventoryManager.instance.AddItemToInventoryInCurrentSlot(reciveItem, itemPacket.amount, CookUI.instance._ingredientSlots[_slotIndex]);
        }
    }

    [PunRPC]
    public void ConfirmRemoveMaterialRequest(string _itemPacket, int _slotIndex, int _viewId)
    {
        ItemPacket itemPacket = JsonUtility.FromJson<ItemPacket>(_itemPacket);
        Item reciveItem = ItemManager.instance.GetMaterialById(itemPacket.itemId);
        if (myPlayerReference.userId == itemPacket.userId && itemPacket.itemId == MaterialSlot.instance.currentCharacterMat.Id)// remove my
        {
            InventoryManager.instance.RemoveItemFromCurrentSlot(reciveItem, itemPacket.amount, CookUI.instance._ingredientSlots[_slotIndex]);
            MaterialSlot.instance.addMaterialAmount();
        }
        if (CookUI.instance.gameObject.activeSelf && _viewId == myPlayerReference.currentCooker.PVVeiwId)// remove remote
        {
            InventoryManager.instance.RemoveItemFromCurrentSlot(reciveItem, itemPacket.amount, CookUI.instance._ingredientSlots[_slotIndex]);
        }
    }
    #endregion

    #region RefrashUI
    [PunRPC]
    public void SendRefreshRequest(string _userId)
    {
        string matInCooker = JsonUtility.ToJson(materialsInCooker);
        photonView.RPC("ConfirmRefrashRequest", RpcTarget.All, _userId, matInCooker);
    }
    [PunRPC]
    public void ConfirmRefrashRequest(string _userId, string _matInCooker)
    {

        materialsInCooker = JsonUtility.FromJson<MaterialListSerializable<ItemStore>>(_matInCooker);



        if (myPlayerReference.userId == _userId)
        {
            for (int i = 0; i < materialsInCooker.materialsStore.Count; i++)
            {
                //Debug.LogError(_userId);

                InventoryManager.instance.AddItemToInventoryInCurrentSlot(ItemManager.instance.GetMaterialById(materialsInCooker.materialsStore[i].itemId),
                 materialsInCooker.materialsStore[i].amount, CookUI.instance._ingredientSlots[materialsInCooker.materialsStore[i].slotIndex]);
            }
        }



    }

    #endregion
    float lestStartCookTime = 0;
    [PunRPC]
    public void StartCookRequest(int _cookerId)
    {
        if (Time.time < lestStartCookTime + 0.2f)
        {
            return;
        }
        lestStartCookTime = Time.time;

        if (ingredients.Count == 2)
        {
            currentRecipe = CheckTwoMatRecipe();
            // Debug.LogError(currentRecipe);
        }
        else if (ingredients.Count == 3)
        {
            currentRecipe = CheckThreeMatRecipe();
            // Debug.LogError(currentRecipe);
        }
        if (ingredients.Count >= 2)
        {
            photonView.RPC("ConfirmStartCookRequest", RpcTarget.All, _cookerId, true, currentRecipe.cookinTime, currentRecipe.name);
            ingredients.Clear();
            materialsInCooker.materialsStore.Clear();
            
        }



    }

    [PunRPC]
    public void ConfirmStartCookRequest(int _cookerId, bool _isCooking, float _cookingTime, string _recipeName)
    {
        //Debug.LogError(string.Format("CookerID: {0} start cooking!", _cookerId));

        if (myPlayerReference.currentCooker != null && myPlayerReference.currentCooker.PVVeiwId == _cookerId)
        {
            isCooking = _isCooking;
            CookUI.instance.gameObject.SetActive(false);

        }
        cookingProgress.gameObject.SetActive(true);
        PotFX.Play();
        StartCoroutine(CookingProgress(_cookingTime, _recipeName));

    }
    float lestPickUpRequestTime = 0f;
    [PunRPC]
    private void PickUpRequest(int _cookerId, string _userId)
    {
        if (Time.time < lestPickUpRequestTime + 0.2f)
            return;
        lestPickUpRequestTime = Time.time;
        isCooking = false;
        photonView.RPC("ConfirmPickUpDish", RpcTarget.All, _cookerId, _userId, isCooking);
    }
    [PunRPC]
    private void ConfirmPickUpDish(int _cookerId, string _userId, bool _isCooking)
    {
        resultIconDisplay.sprite = null;
        resultIconDisplay.gameObject.SetActive(false);
        isCooking = _isCooking;
        
        if (myPlayerReference.userId == _userId && myPlayerReference.currentCooker.PVVeiwId == _cookerId)
        {
            InventoryManager.AddItemToInventory(currentRecipe.resultDish, 1);
        }
    }
  
    [PunRPC]
    private void CookDone(string recepeName)
    {
        PotFX.Stop();
        cookingProgressBar.fillAmount = 0;
        cookingProgress.gameObject.SetActive(false);

        resultIconDisplay.gameObject.SetActive(true);
        currentRecipe = RecipeManager.instance.GetRecipeByName(recepeName);
        resultIconDisplay.sprite = currentRecipe.resultDishImage;
        SFXPlayer.pitch = pitch;
        SFXPlayer.PlayOneShot(dingFX);
    }

}