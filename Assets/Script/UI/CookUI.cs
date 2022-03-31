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
    ItemPacket itemPacket = new ItemPacket();

   
     struct ItemPacket
    {
        public int itemId;
        public int amount;
    }

    int cId;

    public bool _isCookerOpen;
    Vector3 lookAt;

    private void OnEnable()
    {
        //subscribe Cooker
        //EventManager.instance.UIToCooker
        _ingredientSlots[0].addIngredientButtom.onClick.RemoveAllListeners();
        _ingredientSlots[0].addIngredientButtom.onClick.AddListener(delegate { EventManager.instance.CookerToUI(covertedItemPacketJson()); });


    }
    private void OnDisable()
    {
        //Desubscribe Cooker
        //EventManager.instance.UIToCooker
    }

    private void InitPacket()
    {
        itemPacket.itemId = MaterialSlot.instance.currentCharacterMat.Id;
        itemPacket.amount = 1;
       

    }


    private void Awake()
    {
        cam = Camera.main;
        cId = SaveManager.instance.nowData.characterID;
        if (instance == null)
        {
            instance = this;
        }
        

        CheckTeamBG();

    }

    private void Start()
    {
        InitPacket();
       
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
