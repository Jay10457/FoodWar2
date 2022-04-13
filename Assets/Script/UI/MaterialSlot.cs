using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class MaterialSlot : MonoBehaviour
{
    public static MaterialSlot instance;
    public List<GameObject> materials;
    [SerializeField] Item[] materialItems;
    [SerializeField] TMP_Text amountText = null;
    public int materialAmount;
    public Item currentCharacterMat;
    public Action addMaterialAmount;
    public Action reduceMaterialAmount;




    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        
    }
    private void Start()
    {

        currentCharacterMat = materialItems[SaveManager.instance.nowData.characterID];
        materialAmount = 5;
        addMaterialAmount = AddMaterialAmount;
        reduceMaterialAmount = ReduceMaterilaAmount;
        

    }
    private void AddMaterialAmount()
    {
        materialAmount += 1;
        amountText.text = string.Format("{0}/5 ", materialAmount);
    }

    private void ReduceMaterilaAmount()
    {
        materialAmount -= 1;
        amountText.text = string.Format("{0}/5 ", materialAmount);
        if (materialAmount <= 0)
        {
            materialAmount = 0;
            amountText.color = Color.red;
        }
    }

    public void SetMaterialImage(int _id)
    {
        materials[_id].SetActive(true);
    }
}
