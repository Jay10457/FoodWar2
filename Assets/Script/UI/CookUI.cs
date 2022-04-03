using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class CookUI : MonoBehaviour
{
    public static CookUI instance;
    public IngredientSlot[] _ingredientSlots;
    public Button startCooking;
    private Camera cam;
    [SerializeField] GameObject[] BGs;
    [SerializeField] Vector3 offset;
    [SerializeField] CookManager myPlayerRef = null;
  
    ItemPacket itemPacket = new ItemPacket();

   
     struct ItemPacket
    {
        public int itemId;
        public int amount;
        public string userId;
    }

    int cId;

    public bool _isCookerOpen;
    Vector3 lookAt;

    private void OnEnable()
    {
        //subscribe Cooker
        //TODO: Refrash UI from server
        for (int i = 0; i < _ingredientSlots.Length; i++)
        {

        
          
            _ingredientSlots[i].addButtomOnClick = SendSlotIndex;
        }
       


    }
    private void OnDisable()
    {
        //Desubscribe Cooker
       
        for (int i = 0; i < _ingredientSlots.Length; i++)
        {
            if (_ingredientSlots[i].currentItem != null)
            {
                Debug.LogError(string.Format("slot :{0} is {1}", _ingredientSlots[i].index, _ingredientSlots[i].currentItem.name));
                InventoryManager.instance.RemoveItemFromCurrentSlot(_ingredientSlots[i].currentItem, _ingredientSlots[i].currentItemAmount, _ingredientSlots[i]);
            }
           
            
            _ingredientSlots[i].addButtomOnClick -= SendSlotIndex;

        }//EventManager.instance.UIToCooker
    }


    private void SendSlotIndex(int index)
    {
        myPlayerRef.currentCooker.PutMaterialRPC(covertedItemPacketJson(), index);
    }
  
    
    private void InitPacket()
    {
        itemPacket.itemId = MaterialSlot.instance.currentCharacterMat.Id;
        itemPacket.amount = 1;
        itemPacket.userId = RoomManager.instance.myPlayer.userId;
       

    }
  
    private void Awake()
    {
        cam = Camera.main;
        cId = SaveManager.instance.nowData.characterID;
        myPlayerRef = RoomManager.instance.myPlayer;
        if (instance == null)
        {
            instance = this;
        }
        

        CheckTeamBG();

    }

    private void Start()
    {
        InitPacket();
        //Debug.LogError(covertedItemPacketJson());
       
    }

    string covertedItemPacketJson()
    {
        return JsonUtility.ToJson(itemPacket);
    }




    private void CheckTeamBG()
    {
        if (cId <= 4)
        {
            BGs[1].SetActive(true);
        }
        else
        {
            BGs[0].SetActive(true);
        }
    }


    public void SetCookerUIBillboard(Vector3 lookAtPos)
    {

        Vector3 pos = cam.WorldToScreenPoint(lookAtPos) + offset;

        if (transform.position != pos) transform.position = pos;
    }


}
